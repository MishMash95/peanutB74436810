using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using peanut.Common;
using System.Text.RegularExpressions;

namespace peanut.Reader {
    /*
        The ActionReader class contains all code for reading villain information
        and tracing the current last actions of the hand, including getting all
        villain last actions such as bets/folds.

        This class is assumed to only be sampled when it is the bots turn to act.
    */
    class ActionReader {

        // Members
        private Bitmap bitmap;

        // Set local working bitmap
        public void setBitmap( Bitmap bmp ) {
            bitmap = bmp;
        }

        // Fetch villains
        /*
            Fetches all villains with the villain[0] being the bot
            - Hole cards used for determining main player state (i.e folded) using cards as a guide
            - buttonPosition is used to resolve each villains action position (i.e SB, BB, UTG etc)
        */
        public Villain[] getVillains(Card[] holeCards, int buttonPosition) {
            Villain[] villains = new Villain[6]; // Assume 6-max

            if( holeCards == null )  holeCards    = TableReader.cardDetector.RetrievePocketCards();
            if (buttonPosition < 0) buttonPosition = this.getButtonPosition();

            // Define rectangles
            Rectangle[] regionVillain = new Rectangle[6];
            regionVillain[0] = new Rectangle(371, 382, 94, 45);
            regionVillain[1] = new Rectangle(36, 287, 94, 45);
            regionVillain[2] = new Rectangle(36, 115, 94, 45);
            regionVillain[3] = new Rectangle(333, 47, 94, 45);
            regionVillain[4] = new Rectangle(671, 113, 94, 45);
            regionVillain[5] = new Rectangle(671, 285, 94, 45);

            // Get villains
            for (int i = 0; i < 6; i++) {
                villains[i] = getVillainFromRegion(regionVillain[i], i, holeCards, buttonPosition);
            }

            return villains;
        }


        // Extract a villain from the given region
        public Villain getVillainFromRegion( Rectangle imageRegion, int posIndex, Card[] holeCards, int buttonPosition ) {

            // Calculate Card Region (Used to determine if folded)
            Rectangle cardRegion = new Rectangle(imageRegion.X, imageRegion.Y-30, imageRegion.Width, 20 );

            // Read information from image:
            Bitmap bcr  = CardDetection.CopyBitmapSection(bitmap, imageRegion);
            CardDetection.filterBMP2(bcr);
            // Format
            bcr = CardDetection.upscaleBmp(bcr, 3);

            // Scan using image to text engine
            int      stackValue  = 0;
            String[] textRes;
            State    playerState = State.EMPTY_SEAT;
            String   name = "default";
            using (var page = TableReader.engine.Process(bcr)) {

                String result = page.GetText();
                if (result.Length > 0) {
                    textRes = result.Split('\n');

                    name = textRes[0];
                    string stack = textRes[1];

                    // Check if player has folded:
                    if (posIndex > 0) {
                        Color c = CardDetection.sampleBitmapAverageColour(bitmap, cardRegion);
                        if (!CardDetection.colourIsSimilar(c, Color.FromArgb(0xA44644), 75)) {
                            playerState = State.FOLDED;
                        }
                    }

                    // Check if sitting out:

                    Console.WriteLine("u: " + stack);
                    string lc = stack.ToLower();
                    if (lc.Contains("sitting out") || lc.Contains("sit") || lc.Contains("out") || lc.Contains("itti")) {
                        if (playerState == State.FOLDED) {
                            playerState = State.SITTING_OUT;
                        }
                    } else {
                        stack = filterStackString(CardDetection.normaliseToNumberString(stack)); // Remove numbers and nonsense from stack string so it is just a value
                        if ( stack.Length > 0) {
                            if ( playerState != State.FOLDED) {
                                playerState = State.ACTIVE;
                            }

                            
                            Console.WriteLine("f: " + stack);
                            stackValue = Convert.ToInt32(stack);
                        } else {
                            playerState = State.FOLDED;
                        }
                    }

                 
                }
            }

            bcr.Save("villain" + posIndex + ".png", ImageFormat.Png);

            // Fold case for hero
            if( posIndex == 0 && playerState == State.ACTIVE ){
                if (holeCards[0].rank == Card.Rank.NotFound || holeCards[1].rank == Card.Rank.NotFound) {
                    playerState = State.FOLDED;
                }
            }

            // Determine position
            int relativePositionToButton = (posIndex-buttonPosition) % 6;
            Position pos                 = (Position)relativePositionToButton;

            // ----- Construct villain ----- //
            Villain vill = new Villain(name, stackValue, pos, playerState);
            return vill;
        }

        public static string filterStackString(string input) {
            Regex regexObj = new Regex(@"[^\d]");
            return regexObj.Replace(input, "");
        }


        // Returns the location of the button
        /*
            0 = hero
            n = hero+n (clockwise)
        */
        public int getButtonPosition() {

            // Define rectangles
            Rectangle[] buttonRegions = new Rectangle[6];
            buttonRegions[0] = new Rectangle(452, 330, 27, 24);
            buttonRegions[1] = new Rectangle(191, 288, 27, 24);
            buttonRegions[2] = new Rectangle(166, 156, 27, 24);
            buttonRegions[3] = new Rectangle(325, 103, 27, 24);
            buttonRegions[4] = new Rectangle(611, 161, 27, 24);
            buttonRegions[5] = new Rectangle(615, 231, 27, 24);

            for( int i=0; i < 6; i++ ) {
                Color c = CardDetection.sampleBitmapAverageColour(bitmap, buttonRegions[i]);
                //Console.WriteLine(c);
                if( !CardDetection.colourIsSimilar( c, Color.FromArgb(0x027C28), 50) ){
                    return i;
                }
            }
            Console.WriteLine("Error: Button not found!");
            return 0;
        }


    }
}

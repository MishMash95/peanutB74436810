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
        */
        public Villain[] getVillains() {
            Villain[] villains = new Villain[6]; // Assume 6-max

            // Define rectangles
            Rectangle[] regionVillain = new Rectangle[6];
            regionVillain[0] = new Rectangle(371, 381, 94, 40);
            regionVillain[1] = new Rectangle(36, 286, 94, 40);
            regionVillain[2] = new Rectangle(36, 114, 94, 40);
            regionVillain[3] = new Rectangle(333, 46, 94, 40);
            regionVillain[4] = new Rectangle(671, 112, 94, 40);
            regionVillain[5] = new Rectangle(671, 284, 94, 40);

            // Get villains
            for (int i = 0; i < 6; i++) {
                villains[i] = getVillainFromRegion(regionVillain[i], i);
            }

            return villains;
        }


        // Extract a villain from the given region
        public Villain getVillainFromRegion( Rectangle imageRegion, int posIndex ) {

            // Read information from image:
            Bitmap bcr = CardDetection.CopyBitmapSection(bitmap, imageRegion);

            // Format
            bcr = CardDetection.upscaleBmp(bcr, 2);

            // Scan using image to text engine
            int      stackValue  = 0;
            String[] textRes;
            State    playerState = State.EMPTY_SEAT;
            String   name = "default";
            using (var page = TableReader.engine.Process(bcr)) {

                String result = page.GetText();
                if( result.Length > 0 ) {
                    textRes = result.Split('\n');

                    name         = textRes[0];
                    string stack = textRes[1];

                    // Check if sitting out:
                    if( stack.Contains("Sitting Out") ) {
                        playerState = State.SITTING_OUT;
                    } else {
                        playerState = State.ACTIVE;
                        stack      = filterStackString(stack); // Remove numbers and nonsense from stack string so it is just a value
                        stackValue = Convert.ToInt32(stack); 
                    }

                    // Check if player has folded:

                }
            }
            bcr.Save("villain" + posIndex + ".png", ImageFormat.Png);

            // ----- Construct villain ----- //
            Villain vill = new Villain(name, stackValue, Position.B, playerState);
            return vill;
        }

        public static string filterStackString(string input) {
            Regex regexObj = new Regex(@"[^\d]");
            return regexObj.Replace(input, "");
        }


    }
}

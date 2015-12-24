using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using System.Drawing;
using peanut.Common;

namespace peanut.Reader
{

    class TableReader
    {
        // SHARED STATIC ENGINE
        //  - Common reading classes shared by all bots which merely provide functionality
        public static TesseractEngine engine       = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
        public static CardDetection   cardDetector = new CardDetection();
        public static ActionReader    actionReader = new ActionReader();

        // Class attributes
        public Bitmap lastCachedBitmap;

        // Main class body
        /**
            Initialise a table reader.
            Each bot will be associated with a single table reader.
            This class is responsible for allowing the bot to query ALL information on screen.

            A bot is bound to a window handle representing the frame of the game.

            [For testing purposes, we will just hard code in a bitmap source.]
        */
        public TableReader(IntPtr windowHandle){
            
        }

        // Read image
        /*
            Updates the current cached bitmap to a new one from the given table.
        */
        public void readImage() {
            lastCachedBitmap = new Bitmap("testImage.png");// captureWindowFrame(...);
        }


        // Get button position
        /*
            Returns the position of the button as a seat index.
            (from 0 to 5 ) with 0 being the seat at the bottom (i.e where the bot should be)
        */
        public int getButtonPosition() {
            return 0;
        }

        // Get community cards
        /*
            Returns an array of cards containing the current board state.
            If it is preflop, the array will be empty.
        */
        public Card[] getCommunityCards() {
            TableReader.cardDetector.setBitmap(lastCachedBitmap);
            return TableReader.cardDetector.RetieveCommunityCards();
        }

        // Get pocket cards
        /*
            Returns the bots pocket cards. (Assumed that the bot is sitting in the bottom seat)
        */
        public Card[] getPocketCards() {
            // TableReader.cardDetector.
            return null;
        }

        // Get villains
        /*
            Returns an array containing all other villains.
            The index of the array refers to the seat index from 1..5
            (0 will be the bot)

            If a seat is empty, a villain will still be returned, but the state
            of this villain will be set to State.EMPTY_SEAT.

            This is so seat position can still be easily verified
        */
        public Villain[] getVillains() {
            return null;
        }

        // Get ActionStack
        /*
            Returns an action stack containing the action 

        */
        public ActionStack getAction() {
            return null;
        }

    }
}

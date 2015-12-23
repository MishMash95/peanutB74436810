using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace peanut
{
    class CardDetection
    {
        public struct NamedBitmap
        {
            public Bitmap bitmap;
            public string name;
        }

        private List<NamedBitmap> cardPics = new List<NamedBitmap>();
        private List<NamedBitmap> tablePics = new List<NamedBitmap>();

        public void Initialize()
        {
            //Load the Bitmaps into carPics, and tablePics Lists
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.ToUpper().EndsWith(".BMP"))
                {
                    string[] splitter = resourceName.Split('\\');
                    string fileName = splitter[splitter.Length - 1];

                    string[] splitter2 = fileName.Split('.');

                    //if filename Length is 2 or 3.
                    if (splitter2[0].Length == 2 || splitter2[0].Length == 3)
                    {
                        //File name was 2 or 3 chars long, its a card bitmap
                        NamedBitmap b = new NamedBitmap();
                        b.name = splitter2[0];
                        b.bitmap = new Bitmap(resourceName);
                        cardPics.Add(b);
                    }
                    else
                    {
                        //File name was not 2 chars long, its a table bitmap
                        NamedBitmap b = new NamedBitmap();
                        b.name = splitter2[0];
                        b.bitmap = new Bitmap(resourceName);
                        tablePics.Add(b);
                    }
                }
            }
        }

        private Bitmap RetrieveBitmap(string name)
        {
            //Check cardPics List
            foreach (NamedBitmap nb in cardPics)
            {
                if (nb.name.ToUpper() == name.ToUpper())
                {
                    return nb.bitmap;
                }
            }

            //Check tablePics List
            foreach (NamedBitmap nb in tablePics)
            {
                if (nb.name.ToUpper() == name.ToUpper())
                {
                    return nb.bitmap;
                }
            }

            return null;
        }

        public Card[] RetieveCommunityCards()
        {
            Bitmap currentCommunityCards = CaptureCommunityCardsImage();

            int foundCount = 0;
            int cardsToSearch;
            Card[] hand;

            //Check to see how many cards you need to find, if no cards are on the table return null.
            if (SearchBitmap(currentCommunityCards, RetrieveBitmap("3cards")))
            {
                cardsToSearch = 3;
                hand = new Card[3];
            }
            else if (SearchBitmap(currentCommunityCards, RetrieveBitmap("4cards")))
            {
                cardsToSearch = 4;
                hand = new Card[4];
            }
            else if (SearchBitmap(currentCommunityCards, RetrieveBitmap("5cards")))
            {
                cardsToSearch = 5;
                hand = new Card[5];
            }
            else
            {
                return null;
            }

            foreach (NamedBitmap cardImage in cardPics)
            {
                if (SearchBitmap(currentCommunityCards, cardImage.bitmap))
                {
                    //Split the Bitmap filenames up into two seperate strings. EX) filename == S7, then stringSuit = "S", and stringRank = "7"
                    string stringSuit = cardImage.name.Substring(0, 1);
                    string stringRank = cardImage.name.Substring(1);

                    //Get the Bitmap filenames into Suit and Rank Enums. EX) filename == S7, then cardSuit = Suit.Spades, and cardRank = Rank.Seven
                    Card.Suit cardSuit = ConvertStringToSuit(stringSuit);
                    Card.Rank cardRank = ConvertStringToRank(stringRank);

                    hand[foundCount] = new Card(cardSuit, cardRank);
                    foundCount++;

                    //If you found all the cards, stop searching
                    if (foundCount == cardsToSearch)
                    {
                        break;
                    }
                }
            }
            return hand;
        }

        private bool SearchBitmap(Bitmap bigBmp, Bitmap smallBmp)
        {
            double tolerance = 0d;
            BitmapData smallData =
              smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData bigData =
              bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride;
            int bigStride = bigData.Stride;

            int bigWidth = bigBmp.Width;
            int bigHeight = bigBmp.Height - smallBmp.Height + 1;
            int smallWidth = smallBmp.Width * 3;
            int smallHeight = smallBmp.Height;

            Rectangle location = Rectangle.Empty;
            int margin = Convert.ToInt32(255.0 * tolerance);

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0;
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int smallOffset = smallStride - smallBmp.Width * 3;
                int bigOffset = bigStride - bigBmp.Width * 3;

                bool matchFound = true;

                for (int y = 0; y < bigHeight; y++)
                {
                    for (int x = 0; x < bigWidth; x++)
                    {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        //Look for the small picture.
                        for (int i = 0; i < smallHeight; i++)
                        {
                            int j = 0;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++)
                            {
                                //With tolerance: pSmall value should be between margins.
                                int inf = pBig[0] - margin;
                                int sup = pBig[0] + margin;
                                if (sup < pSmall[0] || inf > pSmall[0])
                                {
                                    matchFound = false;
                                    break;
                                }

                                pBig++;
                                pSmall++;
                            }

                            if (!matchFound) break;

                            //We restore the pointers.
                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            //Next rows of the small and big pictures.
                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        //If match found, we return.
                        if (matchFound)
                        {
                            location.X = x;
                            location.Y = y;
                            location.Width = smallBmp.Width;
                            location.Height = smallBmp.Height;
                            break;
                        }
                        //If no match found, we restore the pointers and continue.
                        else
                        {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }

                    if (matchFound) break;

                    pBig += bigOffset;
                }
            }

            bigBmp.UnlockBits(bigData);
            smallBmp.UnlockBits(smallData);

            if (location.Width != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Bitmap CaptureYourHandImage()
        {
            return CopyPartialBitmap(getTableBitMap(), new Rectangle(352, 365, 90, 53));
        }

        private Bitmap CaptureCommunityCardsImage()
        {
            return CopyPartialBitmap(getTableBitMap(), new Rectangle(262, 204, 282, 74));
        }

        private Bitmap CopyPartialBitmap(Bitmap srcBitmap, Rectangle section)
        {
            // Create the new bitmap and associated graphics object
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);

            // Draw the specified section of the source bitmap to the new one
            g.DrawImage(srcBitmap, 0, 0, section, GraphicsUnit.Pixel);

            // Clean up
            g.Dispose();

            // Return the bitmap
            return bmp;
        }

        private Bitmap getTableBitMap()
        {
            return Bitmaps.table1;
        }


        // Converts a string "S", "C", "H", or "D" into the coresponding Card.Suit Enum
        private Card.Suit ConvertStringToSuit(string stringSuit)
        {
            Card.Suit cardSuit;

            //Figure out which Card.Suit the found card was
            switch (stringSuit)
            {
                case "S":
                    cardSuit = Card.Suit.Spades;
                    break;
                case "C":
                    cardSuit = Card.Suit.Clubs;
                    break;
                case "H":
                    cardSuit = Card.Suit.Hearts;
                    break;
                case "D":
                    cardSuit = Card.Suit.Diamonds;
                    break;
                default:
                    cardSuit = Card.Suit.NotFound;
                    break;
            }

            return cardSuit;
        }


        // Converts a string "2", "3", "4"... "J", "Q", "K", or "A"  into the coresponding Card.Rank Enum
        private Card.Rank ConvertStringToRank(string stringRank)
        {
            Card.Rank cardRank;

            //Figure out which Card.Rank the found card was
            switch (stringRank)
            {
                case "2":
                    cardRank = Card.Rank.Two;
                    break;
                case "3":
                    cardRank = Card.Rank.Three;
                    break;
                case "4":
                    cardRank = Card.Rank.Four;
                    break;
                case "5":
                    cardRank = Card.Rank.Five;
                    break;
                case "6":
                    cardRank = Card.Rank.Six;
                    break;
                case "7":
                    cardRank = Card.Rank.Seven;
                    break;
                case "8":
                    cardRank = Card.Rank.Eight;
                    break;
                case "9":
                    cardRank = Card.Rank.Nine;
                    break;
                case "10":
                    cardRank = Card.Rank.Ten;
                    break;
                case "J":
                    cardRank = Card.Rank.Jack;
                    break;
                case "Q":
                    cardRank = Card.Rank.Queen;
                    break;
                case "K":
                    cardRank = Card.Rank.King;
                    break;
                case "A":
                    cardRank = Card.Rank.Ace;
                    break;
                default:
                    cardRank = Card.Rank.NotFound;
                    break;
            }

            return cardRank;
        }
    }
}

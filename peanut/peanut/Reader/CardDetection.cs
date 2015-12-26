using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using peanut.Reader;

namespace peanut
{
    class CardDetection
    {
        // Members
        private Bitmap bitmap;

        // Constructor
        public CardDetection() {

        }

        // Set local working bitmap
        public void setBitmap( Bitmap bmp ) {
            bitmap = bmp;
        }

        // Retrieve Community Cards
        public Card[] RetieveCommunityCards() {
            Card[] communityCards = new Card[5];

            // Get bounds
            GraphicsUnit gu = GraphicsUnit.Pixel;
            RectangleF bounds = bitmap.GetBounds(ref gu);

            // ------------------------------------------------------------------//
            // Define relative ratios ~ For relative reading
            float relativeCardStartX = 0.33477135461f;
            float relativeCardStartRankY = 0.3153942428f;
            float relativeCardWidth = 0.02188868042f;
            float relativeCardRankHeight  = 0.05006257822f;
            //float relativeCardSpacing     = 54.0f / 794.0f;//0.06801007556f;//0.06557377049f;

            // relative spacing
            /*
            Spacing is based on the raw size of the card. There are preset card sizes: ( in pixels)
            
            0 = 30w 42h
            1 = 36w 51h
            2 = 42w 59h
            3 = 50w 70h
            4 = 60w 84h
            5 = 70w 98h
            6 = 84w 118h

            these are decided based on the overall size of the window.

            Examples:

                Table size: 534w 368h uses size 1.
                Table size: 1159w 799h uses size 5

            approximation nearest size: tableWidth/16   
            Spacing should therefore be equal to width of  card + min(max( 3,size), 8);   
            */
            float relativeCardSpacing = 54.0f / 794.0f;

            // Relative suit region (266, 198, 14, 14) (794, 547);
            float relativeSuitRegionX = 266.0f / 794.0f;
            float relativeSuitRegionY = 198.0f / 547.0f;
            float relativeSuitRegionW = 14.0f / 794.0f;
            float relativeSuitRegionH = 14.0f / 547.0f;
            // ------------------------------------------------------------------//

            // Capture:
            for ( int i=0; i < 5; i++ ) {
                Rectangle rankRegion = new Rectangle(   (int)Math.Floor((relativeCardStartX + ((relativeCardSpacing) * i))*(bounds.Width)), 
                                                        (int)Math.Floor(relativeCardStartRankY * bounds.Height), 
                                                        (int)Math.Floor(relativeCardWidth * bounds.Width),
                                                        (int)Math.Floor(relativeCardRankHeight * bounds.Height));

                Rectangle suitRegion = new Rectangle(   (int)Math.Floor((relativeSuitRegionX + (relativeCardSpacing * i)) * (bounds.Width)),
                                                        (int)Math.Floor(relativeSuitRegionY * bounds.Height),
                                                        (int)Math.Floor(relativeSuitRegionW * bounds.Width),
                                                        (int)Math.Floor(relativeSuitRegionH * bounds.Height));
                //Console.WriteLine(suitRegion.ToString());
                //Rectangle rankRegion = new Rectangle(397, 269, 18, 22);
                Bitmap section       = CopyBitmapSection(bitmap, rankRegion);
                //Bitmap sectionSuit = CopyBitmapSection(bitmap, suitRegion);
                //sectionSuit.Save("Suitsec" + i + ".png", ImageFormat.Png);

                // Filter out noise
                filterBMP(section);

                // Stitch multiple sections together (To improve read accuracy)
                Bitmap b = new Bitmap(rankRegion.Width * 4, rankRegion.Height);
                Graphics g = Graphics.FromImage(b);
                g.DrawImage(section, 0, 0);
                g.DrawImage(section, rankRegion.Width, 0);
                g.DrawImage(section, rankRegion.Width*2, 0);
                g.DrawImage(section, rankRegion.Width*3, 0);
                g.Dispose();
                section = b;

                // Perform read
                //filterBMP2(section);
                section = upscaleBmp(section);
                section.Save("Card" + i + ".png", ImageFormat.Png);
                Card.Rank rank    = getCardRankFromBitmap(section);

                // Determine suit


                /*
                    Determines the suit of the given card by sampling the average colour in a given region.
                */
                Color sample   = sampleBitmapAverageColour(bitmap, suitRegion);
                Card.Suit suit = Card.Suit.NotFound;

                if (rank != Card.Rank.NotFound) {
                    /*if (colourIsSimilar(sample, Color.FromArgb(200, 10, 10))) {         suit = Card.Suit.Hearts;      } 
                    else if (colourIsSimilar(sample, Color.Black)) {  suit = Card.Suit.Spades;      } 
                    else if (colourIsSimilar(sample, Color.FromArgb(45, 135, 15))) {  suit = Card.Suit.Clubs;       } 
                    else if (colourIsSimilar(sample, Color.Blue)) {   suit = Card.Suit.Diamonds;    }*/
                    if( (sample.R > 150) || (sample.R > 110 && sample.G < 90 && sample.B < 90 ) ) { suit = Card.Suit.Hearts; }
                    else if( sample.G > 130 ) { suit = Card.Suit.Clubs; }
                    else if( sample.B > 150 ) { suit = Card.Suit.Diamonds; }
                    else if( sample.R < 100 && sample.G < 100 && sample.B < 100 ) { suit = Card.Suit.Spades; }
                }

                communityCards[i] = new Card(suit, rank);
            }
            return communityCards;
        }

        // Copies a section of a bitmap and returns a new bitmap:
        public static Bitmap CopyBitmapSection( Bitmap source, Rectangle region ) {
            return source.Clone(region, PixelFormat.Format32bppArgb);
        }


        // Returns the rank of a given card, given its string.
        private Card.Rank getCardRankFromBitmap(Bitmap source) {
            using( var page = TableReader.engine.Process(source)) {

                string text = page.GetText();

                if( text.Length > 0 ) {
                    text = normaliseToNumberString(text);
                    //Console.WriteLine("detected string: " + text);

                    string convtext = text[0] + "";
                    if( text.Contains( "10" )) {
                        convtext = "T";
                    }

                    return Card.ConvertStringToRank(convtext);
                }
            }
            return Card.Rank.NotFound;
        }

        /*
            Filters out unwanted colours from the bitmap, so we are left with only the desired end colours.
        */
        public static void filterBMP( Bitmap bmp ) {
            GraphicsUnit gu = GraphicsUnit.Pixel;
            RectangleF bounds = bmp.GetBounds(ref gu);

            for( int i=0; i < bounds.Width; i++ ) {
                for( int j = 0; j < bounds.Height; j ++ ) {

                    Color col = bmp.GetPixel(i, j);

                    if( (col.R > 160 && col.G < 100) || (col.R < 40 && col.G < 40 && col.B < 40) || col.G > 120 && col.B < 25 || col.B > 160 && col.R < 40) {
                        bmp.SetPixel(i, j, Color.White);
                    } else {
                        bmp.SetPixel(i, j, Color.Black);
                    }
                }
            }

        }
        public static void filterBMP2(Bitmap bmp) {
            GraphicsUnit gu = GraphicsUnit.Pixel;
            RectangleF bounds = bmp.GetBounds(ref gu);
            for (int i = 0; i < bounds.Width; i++) {
                for (int j = 0; j < bounds.Height; j++) {

                    Color col = bmp.GetPixel(i, j);

                    if (col.R >180 || col.G >180 || col.B > 180) {
                        bmp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    } else {
                        bmp.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                }
            }
        }

        public static Bitmap upscaleBmp( Bitmap source ) {
            GraphicsUnit gu = GraphicsUnit.Pixel;
            int width = (int)source.GetBounds(ref gu).Width;
            int height = (int)source.GetBounds(ref gu).Height;
            Bitmap newb = new Bitmap(width *3, height * 3);
            Graphics g = Graphics.FromImage(newb);
            g.DrawImage(source, new Rectangle(0, 0, width*3, height*3), new Rectangle(0, 0, width, height), gu);
            g.Dispose();
            return newb;
        }

        public static Color sampleBitmapAverageColour(Bitmap source, Rectangle region) {

            int r = 0, g = 0, b = 0;
            int samples = 0;

            for (int i = region.X; i < region.X+region.Width; i++) {
                for (int j = region.Y; j < region.Y+region.Height; j++) {

                    Color col = source.GetPixel(i, j);

                    if (col.R < 250 || col.G < 250 || col.B < 250) {
                        r += col.R;
                        g += col.G;
                        b += col.B;
                        samples++;
                    }
                }
            }

            // Determine average
            r /= samples;
            g /= samples;
            b /= samples;
            return Color.FromArgb(r, g, b);
        }

        // Returns true if two colours are similar
        public static Boolean colourIsSimilar(Color A, Color B) {
            return (Math.Abs(A.R - B.R) < 20 && Math.Abs(A.B - B.B) < 20 && Math.Abs(A.G - B.G) < 20);
        }
        
        /*
        Sometimes the scanner gets confused and things a number is an equivalent letter of the same shape. 
        This function converts all string occurences of similar pattern shapes to numbers.
        */
        public static string normaliseToNumberString(string s) {
           return s.ToUpper().Replace('Z', '2').Replace('O', '0').Replace('I', '1').Replace('S', '5').Replace('G','6').Replace('L','1');
        }

    }
}

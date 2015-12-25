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

            // Define relative ratios ~ For relative reading
            float relativeCardStartX      = 0.33477135461f;
            float relativeCardStartRankY  = 0.3153942428f;
            float relativeCardSuitSampleX = 0.348576f;//404 299
            float relativeCardSuitSampleY = 0.257981f;
            float relativeCardWidth       = 0.06902502157f;
            float relativeCardRankHeight  = 0.05006257822f;
            float relativeCardSpacing     = 0.06557377049f;

            // Capture:
            for ( int i=0; i < 5; i++ ) {
                Rectangle rankRegion = new Rectangle(   (int)((relativeCardStartX + (relativeCardSpacing * i))*(bounds.Width)), 
                                                        (int)(relativeCardStartRankY * bounds.Height), 
                                                        (int)(relativeCardWidth * bounds.Width),
                                                        (int)(relativeCardRankHeight * bounds.Height));
                //Rectangle rankRegion = new Rectangle(397, 269, 18, 22);
                Bitmap section       = CopyBitmapSection(bitmap, rankRegion);
                section.Save("Card" + i + ".png", ImageFormat.Png);
                bitmap.Save("test.png", ImageFormat.Png);

                // Perform read
                filterBMP(section);
                Card.Rank rank = getCardRankFromBitmap(section);
            }
            return communityCards;
        }

        // Copies a section of a bitmap and returns a new bitmap:
        public static Bitmap CopyBitmapSection( Bitmap source, Rectangle region ) {
            /*Bitmap b   = new Bitmap(region.Width, region.Height);
            Graphics g = Graphics.FromImage(b);
            g.DrawImage(source, region);
            g.Dispose();
            return b;*/
            return source.Clone(region, PixelFormat.Format32bppArgb);
        }

        /*
            Filters out unwanted colours from the bitmap, so we are left with only the desired end colours.
        */
        public static void filterBMP( Bitmap bmp ) {
            RectangleF bounds = bmp.GetBounds();

            for( int i=0; i < bounds.Width; i++ ) {
                for( int j = 0; j < bounds.Height; j ++ ) {

                }
            }
        }

    }
}

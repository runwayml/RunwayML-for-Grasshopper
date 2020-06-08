using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization.Formatters;
using Grasshopper.Kernel;


namespace Runway
{
    public class DecomposeImage : GH_Component
    {
      
        public DecomposeImage()
          : base("Decompose Image", "di",
              "Convert data  output to image ",
              "Runway",
              "Image")
        {
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Input Runway Data", "IRD", "input runway Data from Runway component ", GH_ParamAccess.item);
            pManager.AddBooleanParameter( "Run ","R" ," Run convert base 64 to image  ", GH_ParamAccess.item,false);

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("color", "c", "color", GH_ParamAccess.item);
            pManager.AddTextParameter("black & white", "bw", "black & white", GH_ParamAccess.item);
            pManager.AddTextParameter("Red", "R", "Red channel", GH_ParamAccess.item);
            pManager.AddTextParameter("Green", "G", "Green channel", GH_ParamAccess.item);
            pManager.AddTextParameter("Blue", "B", "Blue channel", GH_ParamAccess.item);
            pManager.AddTextParameter("Alpha", "A", "Alpha channel", GH_ParamAccess.item);
            pManager.AddTextParameter("Width", "W", "Width channel", GH_ParamAccess.item);
            pManager.AddTextParameter("Height", "H", "Height channel", GH_ParamAccess.item);

        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
           
            String setdata = "";
            Boolean Reset = false;
            int pixwidth = 0;
            int pixheight = 0;
            List<string> color = new List<string>();
            List<double> blakWhite = new List<double>();
            List<int> red = new List<int>();
            List<int> green = new List<int>();
            List<int> blue = new List<int>();
            List<int> alpha = new List<int>();
            DA.GetData(0, ref setdata);
            DA.GetData(1, ref Reset);

            // convert data

            if (Reset)
            {
                
                //filter json data to base 64 image 
                char[] delimiterChars = {'"', '{', '}', ','};
                string phrase = setdata;
                string[] outputData = phrase.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                string finaltext = outputData[3];
                //convert base 64 to image

                Byte[] bitmapData = Convert.FromBase64String(finaltext);
                System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
                Bitmap bitImage = new Bitmap((Bitmap) Image.FromStream(streamBitmap));

                // width and height image ref to out put
                 pixwidth = bitImage.Width;
                 pixheight = bitImage.Height;


                // decompose image to rgb 


                using (Bitmap bmp = new Bitmap(bitImage))
                {
                    for (int i = 0; i < bitImage.Width; i++)
                    {
                        for (int j = 0; j < bitImage.Height; j++)
                        {
                            Color clr = bmp.GetPixel(i, j);
                           
                            color.Add(clr.R+","+ clr.G+","+clr.B);
                            double redd = double.Parse(clr.R.ToString());
                            double bulee = double.Parse(clr.B.ToString());
                            double grenn = double.Parse(clr.G.ToString());
                            double sum = redd+bulee+grenn;
                            double remap =sum/700;
                            blakWhite.Add(remap);
                            red.Add(clr.R);
                            green.Add(clr.G);
                            blue.Add(clr.B);
                            alpha.Add(255);

                        }

                    }


                }
            }

            //set data to out put component
            DA.SetDataList(0, color);
            DA.SetDataList(1, blakWhite);
            DA.SetDataList(2, red);
            DA.SetDataList(3, green);
            DA.SetDataList(4, blue);
            DA.SetDataList(5, alpha);
            DA.SetData(6, pixwidth);
            DA.SetData(7, pixheight);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                
                return Properties.Resources.Group_26;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("3750da58-fee7-4a96-8791-04929e85648e"); }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Grasshopper.Kernel;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.Geometry.MeshRefinements;

namespace Runway.Properties
{
    public class ShowImage : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the showimage class.
        /// </summary>
        public ShowImage()
          : base("Show image", "sh",
              "convert data  output to image ",
              "Runway",
              "image")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("input runway Data", "ird", "input runway Data from Runway component ", GH_ParamAccess.item);
            pManager.AddBooleanParameter( "run ","r" ," run convert base 64 to image  ", GH_ParamAccess.item,false);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("red", "r", "red channel", GH_ParamAccess.item);
            pManager.AddTextParameter("green", "g", "green channel", GH_ParamAccess.item);
            pManager.AddTextParameter("blue", "b", "blue channel", GH_ParamAccess.item);
            pManager.AddTextParameter("alpha", "a", "alpha channel", GH_ParamAccess.item);
            pManager.AddTextParameter("width", "w", "width channel", GH_ParamAccess.item);
            pManager.AddTextParameter("height", "h", "height channel", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // define input
            String setdata = "";
            Boolean Reset = false;

            int pixwidth = 0;
            int pixheight = 0;

            List<int> red = new List<int>();
            List<int> green = new List<int>();
            List<int> blue = new List<int>();
            List<int> alpha = new List<int>();

            //define input grasshopper component to var

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
                            int redget = clr.R;
                            red.Add(redget);
                            int greenget = clr.G;
                            green.Add(greenget);
                            int blueget = clr.B;
                            blue.Add(blueget);
                            alpha.Add(255);

                        }

                    }


                }
            }

            


            //set data to out put component



            DA.SetDataList(0, red);
            DA.SetDataList(1, green);
            DA.SetDataList(2, blue);
            DA.SetDataList(3, alpha);
            DA.SetData(4, pixwidth);
            DA.SetData(5, pixheight);




        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.image_icon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3750da58-fee7-4a96-8791-04929e85648e"); }
        }
    }
}
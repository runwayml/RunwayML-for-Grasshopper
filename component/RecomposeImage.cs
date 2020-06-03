using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;


namespace Runway

{
    public class CustomParameterAttributes : Grasshopper.Kernel.Attributes.GH_ComponentAttributes
    {
        public Bitmap CanvasImage { get; set; }
        public CustomParameterAttributes(RecomposeImage owner) : base(owner)
        {
            if (CanvasImage == null) CanvasImage = Properties.Resources.RunwayImage;
            
        }

        protected override void Layout()
        {
            Pivot = GH_Convert.ToPoint(Pivot);
            var pivot = Pivot;
            var num1 = (double)pivot.X;
            var num2 = (double)pivot.Y;
            var num3 = 120;
            var num4 = 120;
            m_innerBounds = new RectangleF((float)num1, (float)num2, (float)num3, (float)num4);
            LayoutInputParams(Owner, m_innerBounds);
            LayoutOutputParams(Owner, m_innerBounds);
            Bounds = LayoutBounds(Owner, m_innerBounds);


        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
        

            if (channel != GH_CanvasChannel.Objects) base.Render(canvas, graphics, channel);
            else
            {
                RenderComponentCapsule(canvas, graphics, true, false, false, true, true, false);
                var rectangle1 = GH_Convert.ToRectangle(m_innerBounds);
                if (GH_Canvas.ZoomFadeLow > 0) graphics.DrawImage(CanvasImage, rectangle1);
                if (GH_Canvas.ZoomFadeLow < .67) graphics.DrawImage(Properties.Resources.RunwayImage, rectangle1);
            }
        }
    }
    public class RecomposeImage : GH_Component
    {
        public RecomposeImage()
          : base("Recompose Image", "Ri",
              "get ARGB to Image",
              "Runway", "Image")
        {
        }
        public override void CreateAttributes()
        {
            m_attributes = new CustomParameterAttributes(this);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Red", "R", "Red channel", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Green", "G", "Green channel", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Blue", "B", "Blue channel", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Alpha", "A", "Alpha channel", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Width", "W", "Width channel", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Height", "H", "Height channel", GH_ParamAccess.item);
            pManager.AddBooleanParameter("save", "s", "Save image", GH_ParamAccess.item,false);
            pManager.AddTextParameter("Address", "Ad", "save picture", GH_ParamAccess.item,"");
            pManager.AddTextParameter("file name", "n", "fine name", GH_ParamAccess.item,"default");
        }

       
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("base64", "b64", "Input runway Data from Runway component ", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<int> red = new List<int>();
            List<int> green = new List<int>();
            List<int> blue = new List<int>();
            List<int> alpha = new List<int>();
            int width = 100;
            int height = 100;
            bool blooansave = false;
            string addressPhoto = "";
            string namePhoto= "";
            DA.GetDataList(0, red);
            DA.GetDataList(1, green);
            DA.GetDataList(2, blue);
            DA.GetDataList(3, alpha);
            DA.GetData(4, ref width);
            DA.GetData(5, ref height);
            DA.GetData(6, ref blooansave);
            DA.GetData(7, ref addressPhoto);
            DA.GetData(8, ref namePhoto);

            Bitmap output = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int m = y * width + x;


                    int a = alpha.ElementAt(m);
                    int r = red.ElementAt(m);
                    int g = green.ElementAt(m);
                    int b = blue.ElementAt(m);

                    var setcolor = Color.FromArgb(a, r, g, b);
                    output.SetPixel(x, y, setcolor);
                    //If a invalid color was read in from the config file use white instead
                }
            }
            var atts = Attributes as CustomParameterAttributes;
            atts.CanvasImage = output;

            ImageConverter converter = new ImageConverter();
            byte[] outputt = (byte[])converter.ConvertTo(output, typeof(byte[]));
            string imagebase64 = Convert.ToBase64String(outputt);
            DA.SetData(0, imagebase64);
            if (blooansave)
            {
                string finalAddress = addressPhoto + @"\" + namePhoto+".jpeg"; 
                output.Save(finalAddress);
            }

        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Group_29;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("3bbf701d-9f4c-4381-a0ef-5b9c4c35bc05"); }
        }
    }
}
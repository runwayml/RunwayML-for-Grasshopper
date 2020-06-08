using System;
using System.Drawing;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
namespace image_sampeler
{
    public class CustomParameterAttributes : Grasshopper.Kernel.Attributes.GH_ComponentAttributes
    {

        public Bitmap CanvasImage { get; set; }

        public CustomParameterAttributes(Showimage owner) : base(owner)
        {
            if (CanvasImage == null)
            {
                CanvasImage = Runway.Properties.Resources.RunwayImage;
            }

        }

        private RectangleF imagefarame;
        protected override void Layout()
        {
            Pivot = GH_Convert.ToPoint(Pivot);
            var pivot = Pivot;

            imagefarame = new RectangleF(pivot.X, pivot.Y, CanvasImage.Width, CanvasImage.Height);
            m_innerBounds = imagefarame;
            m_innerBounds.Inflate(-10, 10);
            LayoutInputParams(Owner, m_innerBounds);
            LayoutOutputParams(Owner, m_innerBounds);
            Bounds = LayoutBounds(Owner, m_innerBounds);


        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel != GH_CanvasChannel.Objects)
            {
                base.Render(canvas, graphics, channel);
            }
            else
            {
                RenderComponentCapsule(canvas, graphics, true, false, false, true, false, false);
                var rectangle1 = GH_Convert.ToRectangle(m_innerBounds);
                if (GH_Canvas.ZoomFadeLow > 0)
                {
                    graphics.DrawImage(CanvasImage, imagefarame);
                }
                if (GH_Canvas.ZoomFadeLow < .67)
                {
                    graphics.DrawImage(Runway.Properties.Resources.RunwayImage, imagefarame);
                }

            }

        }
    }

    public class Showimage : GH_Component
    {

        public Showimage()
            : base("Show Image", "SI",
                "Convert base 64 to image",
                "Runway", "Image")
        {
        }

        public override void CreateAttributes()
        {
            m_attributes = new CustomParameterAttributes(this);
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Input Data", "ID", " ", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Out Data", "OD", " ", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string img = "";
            var atts = Attributes as CustomParameterAttributes;
            DA.GetData(0, ref img);
            char[] delimiterChars = { '"', '{', '}', ',' };
            string[] outputData = img.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
            string finaltext = outputData[3];
            Byte[] bitmapData = Convert.FromBase64String(finaltext);
            System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
            Bitmap bitImage = new Bitmap((Bitmap)Image.FromStream(streamBitmap));
            atts.CanvasImage = bitImage;


        }
        protected override System.Drawing.Bitmap Icon
        {
            get { return Runway.Properties.Resources.Group_24; }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("d9b5fb91-b9c8-415f-a426-81a397d61cbb"); }
        }
    }

}

using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Runway.component
{
    public class TakePic : GH_Component
    {
        public TakePic()
          : base("take a photo", "take",
              "take a image from rhino active veiw port",
              "Runway", "Image")
        {
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("width", "w", "width of picture", GH_ParamAccess.item, 400);
            pManager.AddIntegerParameter("height", "h", "height of picture", GH_ParamAccess.item, 400);
            pManager.AddTextParameter("file name", "n", "fine name", GH_ParamAccess.item);
            pManager.AddTextParameter("file address", "ad", "file address", GH_ParamAccess.item);
            pManager.AddIntegerParameter("type", "t", "0 = jpeg 1= png 2=tif", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Reset", "r", "reset picture", GH_ParamAccess.item, false);

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("picture", "p", "address picture", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

           
            int sizeW = 0;
            int sizeH = 0;
            string Name = null;
            string Address = null;
            int type = 0;
            string Type = null;
            bool Run = false;

            DA.GetData(0, ref sizeW);
            DA.GetData(1, ref sizeH);
            DA.GetData(2, ref Name);
            DA.GetData(3, ref Address);
            DA.GetData(4, ref type);
            DA.GetData(5, ref Run);
            if (Run != false)
            {
                switch (type)
                {
                    case 0:
                        Type = ".jpeg";
                        break;
                    case 1:
                        Type = ".png";
                        break;
                    case 2:
                        Type = ".tif";
                        break;

                }

                string finalAddress =Address + @"\" +Name+Type;
                DA.SetData(0, finalAddress);
                var View = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;
                var bitmap = View.CaptureToBitmap(new Size(sizeW, sizeH));
                bitmap.Save(finalAddress);

            }



        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.takimage_icon;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("d838c9b4-4b75-4e8f-84eb-3b7e30e01093"); }
        }
    }
}
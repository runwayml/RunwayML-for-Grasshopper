using System;
using Grasshopper.Kernel;


namespace Runway
{
    public class RunwayComponent : GH_Component
    {
      
        public RunwayComponent()
          : base(
              "get data to runway",
              "RS",
              "Please enter the http address.",
              "Runway",
              "Data")
        {
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Input http", "it", "example http://localhost:8000", GH_ParamAccess.item, "");
            pManager.AddBooleanParameter("Data", "d", "show data output", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Info", "i", "show data output", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Error", "e", "show data output", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Run", "R", "Run ", GH_ParamAccess.item, false);
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Output runway Data", "ord", "output runway live data", GH_ParamAccess.item);
            pManager.AddTextParameter("Output runway Info", "ori", "output runway live data", GH_ParamAccess.item);
            pManager.AddTextParameter("Output runway Error", "ore", "output runway live data", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            // define parameter

            string mainAddress= "";
            bool dataAddress = false;
            bool Binfo = false;
            bool Berror = false;
            bool Brun = false;
            DA.GetData(0, ref mainAddress);
            DA.GetData(1, ref dataAddress);
            DA.GetData(2, ref Binfo);
            DA.GetData(3, ref Berror);
            DA.GetData(4, ref Brun);

            //

            using (System.Net.WebClient client = new System.Net.WebClient()) {
                string clientMain = client.DownloadString(mainAddress);

                if (Brun == true && clientMain != "") {
                    ExpireSolution(true);
                    if (dataAddress != false)
                    {
                        string data = client.DownloadString(mainAddress + "/data");
                        DA.SetData(0, data);
                    }

                    if (Binfo != false)
                    {
                        string infodata = client.DownloadString(mainAddress + "/info");
                        DA.SetData(1, infodata);
                    }
                    if (Berror != false)
                    {
                        string data = client.DownloadString(mainAddress + "/error");
                        DA.SetData(2, data);
                    }

                }

            }

        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                
                return Properties.Resources.Group_25;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("a9c05115-f56a-44d9-bd7b-03dc5832a01f"); }
        }
    }
}

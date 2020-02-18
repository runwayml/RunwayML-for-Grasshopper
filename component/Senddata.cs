using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Grasshopper.Kernel;
using Rhino.Geometry;


// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Runway
{
    public class SendData : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SendData()
          : base("send data to runway", "sd",
              "convert data to json and send to runway",
              "Runway", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("http address", "HA", "http address model from runway", GH_ParamAccess.item);
            pManager.AddTextParameter("Name Category", "NC", "name of category runway model", GH_ParamAccess.item);
            pManager.AddTextParameter("Data ", "D", "Send to Runway", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "R", "Run", GH_ParamAccess.item, false);
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Data in", "D", "Data from Runway", GH_ParamAccess.item);

        }


        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string address = null;
            string name = null;
            string data = null;
            Boolean run = false;


            DA.GetData(0, ref address);
            DA.GetData(1, ref name);
            DA.GetData(2, ref data);
            DA.GetData(3, ref run);
            string addressfinal = address + "/query";
            if (run)
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(addressfinal);
              


                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {


                    string vahid = "{\"" + name + "\":";

                    string mamad = "\"" + data + "\"}";
                    string json = vahid + mamad;

                    streamWriter.Write(json);
                    DA.SetData(0, json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                }
            }

        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Properties.Resources.Group_23;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("87d7628d-9063-4c5b-9ace-abf2763ade10"); }
        }
    }
}
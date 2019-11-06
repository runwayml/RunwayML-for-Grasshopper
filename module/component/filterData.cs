using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Text.RegularExpressions;
using System.Linq;

namespace Runway
{
    public class FilterDataComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public FilterDataComponent()
          : base(
              "FilterData",
              "FD",
              "Filter data by input value",
              "Runway",
              "FilterData")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("input data", "id", "input data from Receive", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("number in model", "nm", "float data receive", GH_ParamAccess.list);
            pManager.AddTextParameter("name category", "ng", "name category from data", GH_ParamAccess.list);
            pManager.AddTextParameter("output data", "od", "receive all data from model in runway", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string inputData = "";
            DA.GetData(0, ref inputData);
            //filter data
            char[] delimiterChars = { ',', '[', ']', '\"', '{', '}', ':' };
            string phrase = inputData;
            string[] outputData = phrase.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);


            //filter text
            List<string> outputText = new List<string>();

            string[] textRegex = Regex.Split(inputData, "[^a-zA-Z]+");

            foreach (var i in textRegex)
            {
                outputText.Add(i);
            }

            outputText = outputText.Where(x => !string.IsNullOrEmpty(x)).ToList();
            //filter number
            List<string> outputNum = new List<string>();
            string[] numRexex = Regex.Split(inputData, @"[^\.0-9]+");

            foreach (var numi in numRexex)
            {
                outputNum.Add(numi);
            }
            outputNum = outputNum.Where(x => !string.IsNullOrEmpty(x)).ToList();


            //set data;

            DA.SetDataList(0, outputNum);
            DA.SetDataList(1, outputText);
            DA.SetDataList(2, outputData);






        }

        /// <summary>

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Properties.Resources.filter; 
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8be70209-fca8-41ca-9952-ac018d7a7082"); }
        }
    }
}
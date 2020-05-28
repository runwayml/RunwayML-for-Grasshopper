using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using System.Text.RegularExpressions;
using System.Linq;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel.Attributes;

namespace Runway
{
    public class FilterDataComponent_custom : GH_ComponentAttributes
    {
        public FilterDataComponent_custom(IGH_Component component) : base(component) { }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            GH_PaletteStyle styleStandard = null;
            GH_PaletteStyle styleSelected = null;

            
            

            if (channel == GH_CanvasChannel.Objects)
            {
                styleStandard = GH_Skin.palette_normal_standard;
                styleSelected = GH_Skin.palette_normal_selected;
                GH_Skin.palette_normal_standard = new GH_PaletteStyle(Color.Black, Color.LightSalmon, Color.DarkSlateGray);
                GH_Skin.palette_normal_selected = new GH_PaletteStyle(Color.SkyBlue, Color.DarkBlue, Color.Black);
                // Restore the cached styles.
                base.Render(canvas, graphics, channel);
                GH_Skin.palette_normal_standard = styleStandard;
                GH_Skin.palette_normal_selected = styleSelected;

            }
            else
            {
                base.Render(canvas, graphics, channel);
            }


        }
    }

    public class FilterDataComponent : GH_Component
    {
        public FilterDataComponent()
          : base(
              "Filter Data",
              "FD",
              "filter Data by input value",
              "Runway",
              "Filters")
        {
        }

        public override void CreateAttributes()
        {
            base.m_attributes = new FilterDataComponent_custom(this);
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Input Data", "I", "Input data from Receive ", GH_ParamAccess.item);

        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("number in model", "nm", "float data receive", GH_ParamAccess.list);
            pManager.AddTextParameter("name category", "ng", "name category from data", GH_ParamAccess.list);
            pManager.AddTextParameter("output data", "od", "receive all data from model in runway", GH_ParamAccess.list);
        }
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
                return Properties.Resources.Group_28; 
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("8be70209-fca8-41ca-9952-ac018d7a7082"); }
        }
    }
}
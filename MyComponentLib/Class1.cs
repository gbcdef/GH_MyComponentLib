using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GH_IO;
using Grasshopper.Kernel;
using Rhino;

namespace MyComponentLib
{
    public class RetrieveListAB:GH_Component
    {
        public RetrieveListAB() :base("RetriveListAB", "ListAB", "Retrive a list's ealier part and later part.", "Sets", "List")
        {
        }

        protected override Bitmap Icon 
        { 
            
            get { return Properties.Resources.ListAB_icon; }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("List","L","List to retrive from.",GH_ParamAccess.list);
            pManager.AddIntegerParameter("Shift", "S", "Shift strength.", GH_ParamAccess.item, 1);
            pManager.AddBooleanParameter("Circular", "O", "If the data list is circular", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Earlier", "A", "The earlier part of the list.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Later", "B", "The later part of the list.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {


            List<Object> l = new List<Object>();
            List<Object> l1 = new List<Object>();
            List<Object> l2 = new List<Object>();
            int s = 0;
            bool cir = false;

            
            if(!DA.GetDataList(0, l)) return;
            if (!DA.GetData(1, ref s)) return;
            if (!DA.GetData(2, ref cir)) return;
            
            int length=l.Count;
            l1 = l.GetRange(0, length - s);
            l2 = l.GetRange(s, length - s);
            if (cir)
            {
                //l1.InsertRange(0, l.GetRange(length - s, s));
                l1 = l;
                l2.AddRange(l.GetRange(0,s));
            }

            DA.SetDataList(0, l1);
            DA.SetDataList(1, l2);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("aa0e907d-e173-4d25-87f2-61dfc8a9c82a"); }
        }
    }

    public class ShrinkDataTree : GH_Component
    {
        public ShrinkDataTree(): base("Shrink Tree", "ShrinkSquare", "Shrink a data tree both in branches and lists", "Sets", "Tree")
        {

        }

        
        protected override Bitmap Icon
        {

            get { return Properties.Resources.ShrinkDataTree_icon; }
        }

        protected override void  RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", "Data tree", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("ListShrink", "Ls", "List shrink strength", GH_ParamAccess.item,1);
            pManager.AddIntegerParameter("BranchShrink", "Bs", "Branch shrink strength", GH_ParamAccess.item,1);
        }

        protected override void  RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", "Shrinked data tree", GH_ParamAccess.tree);
        }

        protected override void  SolveInstance(IGH_DataAccess DA)
        {
 	        //DataTree<object> x, int y, int z, ref object A
            Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.IGH_Goo> x=new Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.IGH_Goo>();
            Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.IGH_Goo> resultTree = new Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.IGH_Goo>();

            int y=0, z=0;
            
            if (!DA.GetDataTree(0,out x)) return;
            if (!DA.GetData(1,ref y)) return;
            if (!DA.GetData(2,ref z)) return;

            resultTree = x.Duplicate();
            

            int listLength = 0;
            int branchCount = x.PathCount;
            int tempy = y;

            
            
            for (int i = 0; i < branchCount; i++)
            {
                IList templist = resultTree.get_Branch(i);
                listLength = templist.Count;

                if (listLength >= y * 2)
                {
                    for (int j = 0; j < y; j++)
                    {
                        templist.RemoveAt(listLength - 1);
                        templist.RemoveAt(0);

                        listLength = templist.Count;

                    }
                }

            }

            
            if (branchCount >= z * 2)
            {
                for (int i = 0; i < z; i++)
                {
                    resultTree.RemovePath(resultTree.get_Path(branchCount - 1));

                    resultTree.RemovePath(resultTree.get_Path(0));
                    branchCount = resultTree.PathCount;
                }


            }

            DA.SetDataTree(0, resultTree);
        }


        public override Guid  ComponentGuid
        {
	        get { return new Guid("9ac9fdb2-b160-48d1-86c5-eab70b9125a2"); }
        }
}
}

﻿// Dxflib
// LwPolyLineBuffer.cs
// 
// ============================================================
// 
// Created: 2018-08-07
// Last Updated: 2018-08-12-2:24 PM
// By: Adam Renaud
// 
// ============================================================

using System.Collections.Generic;
using Dxflib.Geometry;
using Dxflib.Parser;

namespace Dxflib.Entities
{
    /// <inheritdoc />
    /// <summary>
    ///     The Buffer class for holding lwpolyling information during the extraction process
    /// </summary>
    public class LwPolyLineBuffer : EntityBuffer
    {
        /// <inheritdoc />
        /// <summary>
        ///     The Lwpolyline Buffer Constructor that holds all information
        ///     for the Lwpolyline class before it is built
        /// </summary>
        public LwPolyLineBuffer()
        {
            LayerName = "";
            Handle = "";
            NumberOfVertices = 0;
            PolyLineFlag = false;
            ConstantWidth = 0;
            Elevation = 0;
            Thickness = 0;
            XValues = new List<double>();
            YValues = new List<double>();
            BulgeList = new List<double>();
        }

        /// <summary>
        ///     The Number of Vertices in the Lwpolyline
        /// </summary>
        public int NumberOfVertices { get; private set; }

        /// <summary>
        ///     The Lwpolyline Flag, tells if the lwpolyline is open or closed
        /// </summary>
        public bool PolyLineFlag { get; private set; }

        /// <summary>
        ///     Constant width or Global width
        /// </summary>
        public double ConstantWidth { get; private set; }

        /// <summary>
        ///     The elevation of the Lwpolyline
        /// </summary>
        public double Elevation { get; private set; }

        /// <summary>
        ///     The Thickness of the lwpolyline
        /// </summary>
        public double Thickness { get; private set; }

        /// <summary>
        ///     The X values list
        /// </summary>
        public List<double> XValues { get; }

        /// <summary>
        ///     The Y Values List
        /// </summary>
        public List<double> YValues { get; }

        /// <summary>
        ///     The bulge List
        /// </summary>
        public List<double> BulgeList { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Main Parse Function for the Lwpolyline Class
        /// </summary>
        /// <param name="args">LineChangeHandlerArguments</param>
        /// <returns>True or false if the parse was successful</returns>
        public override bool Parse(LineChangeHandlerArgs args)
        {
            if ( base.Parse(args) )
                return true;

            switch ( args.NewCurrentLine )
            {
                // Number of Vertices
                case LwPolyLineGroupGroupCodes.NumberOfVertices:
                    NumberOfVertices = int.Parse(args.NewNextLine);
                    return true;

                // Lwpolyline Flag
                case LwPolyLineGroupGroupCodes.PolyLineFlag:
                    switch ( int.Parse(args.NewNextLine) )
                    {
                        case 0:
                            PolyLineFlag = false;
                            break;
                        case 1:
                            PolyLineFlag = true;
                            break;
                        default:
                            return false;
                    }

                    return true;

                // Constant Width
                case LwPolyLineGroupGroupCodes.ConstantWidth:
                    ConstantWidth = double.Parse(args.NewNextLine);
                    return true;

                // Elevation
                case LwPolyLineGroupGroupCodes.Elevation:
                    Elevation = double.Parse(args.NewNextLine);
                    return true;

                // Thickness
                case LwPolyLineGroupGroupCodes.Thickness:
                    Thickness = double.Parse(args.NewNextLine);
                    return true;

                // X values
                case LwPolyLineGroupGroupCodes.XValue:
                    BulgeList.Add(Bulge.BulgeNull);
                    XValues.Add(double.Parse(args.NewNextLine));
                    return true;

                // Y values
                case LwPolyLineGroupGroupCodes.YValue:
                    YValues.Add(double.Parse(args.NewNextLine));
                    return true;

                // Bulge Values
                case LwPolyLineGroupGroupCodes.Bulge:
                    BulgeList.RemoveAt(BulgeList.Count - 1);
                    BulgeList.Add(double.Parse(args.NewNextLine));
                    return true;

                // ReSharper disable once RedundantEmptySwitchSection
                // This is required for unexpected behaviour
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Class that holds all of the constant strings
        ///     for the lwpolyline group codes.
        /// </summary>
        private static class LwPolyLineGroupGroupCodes
        {
            public const string NumberOfVertices = " 90";
            public const string PolyLineFlag = " 70";
            public const string ConstantWidth = " 43";
            public const string Elevation = " 38";
            public const string Thickness = " 39";
            public const string XValue = " 10";
            public const string YValue = " 20";
            public const string Bulge = " 42";
        }
    }
}
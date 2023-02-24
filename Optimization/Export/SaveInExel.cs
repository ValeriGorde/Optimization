using Microsoft.Office.Interop.Excel;
using Optimization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Export
{
    internal class SaveInExel
    {
        private static Microsoft.Office.Interop.Excel.Application? _Excel;
        private static Workbook? _Workbook;
        private static Worksheet? _Worksheet;

        public static void Export(OptimizationMethod currentMethod, Assignment currentAssignmnt, InputParameter inputParams, OutputParams outputParams) 
        {
            _Excel = new Microsoft.Office.Interop.Excel.Application();
            _Workbook = _Excel.Workbooks.Add();
            _Worksheet = (Worksheet)_Workbook.ActiveSheet;
            _Worksheet.Columns.AutoFit();
            _Worksheet.Name = "Отчет";

            _Worksheet.Cells[1, 1] = "Метод оптимизации";
            _Worksheet.Cells[1, 1].Font.Bold = true;
            _Worksheet.Cells[1, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[1, 2] = currentMethod.Name;
            _Worksheet.Cells[1, 2].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[3, 1] = "Задание";
            _Worksheet.Cells[3, 1].Font.Bold = true;
            _Worksheet.Cells[3, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[3, 2] = currentAssignmnt.Name;
            _Worksheet.Cells[3, 2].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[5, 1] = "Входные параметры";
            _Worksheet.Cells[5, 1].Font.Bold = true;
            _Worksheet.Range[_Worksheet.Cells[5, 1], _Worksheet.Cells[5, 2]].Merge();
            _Worksheet.Cells[5, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[5, 2].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[6, 1] = "Высота теплообменника, м";
            _Worksheet.Cells[6, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[6, 2] = inputParams.H;
            _Worksheet.Cells[6, 2].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[7, 1] = "Число витков змеевика, шт";
            _Worksheet.Cells[7, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[7, 2] = inputParams.N;
            _Worksheet.Cells[7, 2].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[8, 1] = "Нормирующие множители"; // добавить еще два
            _Worksheet.Cells[8, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[8, 2] = inputParams.Alpha;
            _Worksheet.Cells[8, 2].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[10, 1] = "Ограничения";
            _Worksheet.Cells[10, 1].Font.Bold = true;
            _Worksheet.Cells[10, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[11, 1] = inputParams.LMin + " < L < " + inputParams.LMax;
            _Worksheet.Cells[12, 1] = inputParams.SMin + " < S < " + inputParams.SMax;
            _Worksheet.Cells[13, 1] = "0.5T1 + T2 <= " + inputParams.LSSum;
            _Worksheet.Cells[11, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[12, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[13, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[1, 4] = "Выходные параметры";
            _Worksheet.Cells[1, 4].Font.Bold = true;
            _Worksheet.Range[_Worksheet.Cells[1, 4], _Worksheet.Cells[1, 5]].Merge();
            _Worksheet.Cells[1, 4].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[1, 5].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[2, 4] = "Себестоимость теплообменника, у.е.";
            _Worksheet.Cells[2, 4].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[2, 5] = Math.Round(outputParams.CostPriceResult, 2);
            _Worksheet.Cells[2, 5].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[3, 4] = "Ширина теплообменника, м";
            _Worksheet.Cells[3, 4].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[3, 5] = Math.Round(outputParams.WidthResult, 2);
            _Worksheet.Cells[3, 5].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[4, 4] = "Длина теплоообменника, м";
            _Worksheet.Cells[4, 4].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[4, 5] = Math.Round(outputParams.LengthResult, 2);
            _Worksheet.Cells[4, 5].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Cells[1, 7] = "Длина, м";
            _Worksheet.Cells[1, 7].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[1, 8] = "Ширина, м";
            _Worksheet.Cells[1, 8].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Cells[1, 9] = "Себестоимость теплоообменника, у.е.";
            _Worksheet.Cells[1, 9].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            for (int i = 0; i < outputParams.OutputParamsArr.Length; i++)
            {
                _Worksheet.Cells[i + 2, 7] = outputParams.OutputParamsArr[i].Length;
                _Worksheet.Cells[i + 2, 8] = outputParams.OutputParamsArr[i].Width;
                _Worksheet.Cells[i + 2, 9] = outputParams.OutputParamsArr[i].CostPrice;

                _Worksheet.Cells[i + 2, 7].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
                _Worksheet.Cells[i + 2, 8].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
                _Worksheet.Cells[i + 2, 9].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            }
        }

    }
}

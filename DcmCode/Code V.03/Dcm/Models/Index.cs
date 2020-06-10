using Dcm.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dcm.Models
{
    public class Index
    {
        public List<AllTaskStatusChartSourceData> allTaskStatusChartSourceData { get; set; }

        public List<MyAssignedTaskStatusChartSourceData> myAssignedTaskStatusChartSourceData { get; set; }

        public List<MyTaskStatusChartSourceData> myTaskStatusChartSourceData { get; set; }

        public List<AllCompletedTaskByMonthChartSourceData> allCompletedTaskByMonthChartSourceData { get; set; }
        public List<AllCompletedTaskByMonthChartOnTimeSourceData> allCompletedTaskByMonthChartOnTimeSourceData { get; set; }

        public List<AllClosedTaskByMonthChartSourceData> allClosedTaskByMonthChartSourceData { get; set; }
        public List<Calendar> calendar { get; set; }

        public string totalActiveTask { get; set; }

        public string totalActiveFinishedTask { get; set; }

        public string totalActiveClosedTask { get; set; }

        public string totalActiveProcessedTask { get; set; }

        public string totalActiveTaskMy { get; set; }

        public string totalActiveFinishedTaskMy { get; set; }

        public string totalActiveClosedTaskMy { get; set; }

        public string totalActiveProcessedTaskMy { get; set; }
    }

    public class AllTaskStatusChartSourceData
    {
        public string label { get; set; }
        public int value { get; set; }
        public string color { get; set; }
        public string hightlight { get; set; }
    }

    public class MyAssignedTaskStatusChartSourceData
    {
        public string label { get; set; }
        public int value { get; set; }
        public string color { get; set; }
        public string hightlight { get; set; }
    }

    public class MyTaskStatusChartSourceData
    {
        public string label { get; set; }
        public int value { get; set; }
        public string color { get; set; }
        public string hightlight { get; set; }
    }

    public class AllCompletedTaskByMonthChartSourceData
    {
        public string label { get; set; }
        public int value { get; set; }
        public string color { get; set; }
        public string hightlight { get; set; }
    }

    public class AllCompletedTaskByMonthChartOnTimeSourceData
    {
        public string label { get; set; }
        public int value { get; set; }
        public string color { get; set; }
        public string hightlight { get; set; }
    }

    public class AllClosedTaskByMonthChartSourceData
    {
        public string label { get; set; }
        public int value { get; set; }
        public string color { get; set; }
        public string hightlight { get; set; }
    }

    public class Calendar
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string backgroundColor { get; set; }

        public string url { get; set; }
    }
}
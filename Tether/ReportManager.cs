namespace Tether
{
    internal interface IReportManager
    {
        void ReportResult(bool isSuccess);

        void ReportValue(long value, long maxSuccessValue, long maxWarningValue);

        void Report(string message, MessageType messageType);
    }
}
namespace WashingMachineServiceWPF.Services
{
  public  class RecognitionResultEvent
  {
      public RecognitionResultEvent(string message)
      {
          Message = message;
      }

      public string Message { get; private set; }
  }
}

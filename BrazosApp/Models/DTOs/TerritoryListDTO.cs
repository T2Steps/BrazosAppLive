namespace BrazosApp.Models.DTOs
{
    public class TerritoryListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public List<TerritoryInspectorList> territoryInspectorLists { get; set; }
    }

    public class TerritoryInspectorList
    {
        public int AssignedUserId { get; set; }
        //public string AssignedUserName { get; set; }
    }

      //public class SignatureFileRequestDTO
      //{
      //      public string? base64String { get; set; }

      //      public string? userId { get; set; }
      //}
}

namespace Quote.Modal
{
    public class RequestStatusResqonse
    {
        public int requestId { get; set; }
        public string status { get; set; }
        public string? dateSurvey { get; set; }
        public string? dateCreate { get; set; }
        public string? address { get; set; }

        public RequestUser? UserData { get; set; }
        public RequestContrac? ContracData { get; set; }
        public RequestProdcuct? ProdcuctData { get; set; }
        public RequestStaff? StaffData { get; set; }

    }
    public class RequestUser
    {
        public int? userId { get; set; }
        public string? userName { get; set; }
        public string? userEmail { get; set; }
        public string? userPhone { get; set; }

    }
    public class RequestStaff
    {
        public int staffId { get; set; }
        public string? staffName { get; set; }
        public string? staffEmail { get; set; }
        public string? staffPhone { get; set; }

    }
    public class RequestProdcuct
    {
        public int? productId { get; set; }
        public string? productName { get; set; }
        public string? productPrice{ get; set; }
        public string? productCate { get; set; }
        public string? productThumbnail { get; set;}

    }
    public class RequestContrac
    {
        public int? contractId { get; set;}
        public string? priceProduct { get; set; }
        public string? priceConstruc { get; set; }
        public string? contracFile { get; set; }
        public string? status { get; set; }

    }
}

using System;

namespace SevenDigital.Api.Schema.ParameterDefinitions.Post
{
	public class AddCardParameters
	{
		public string Number { get; set; }
		public string Type { get; set; }
		public string HolderName { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime ExpiryDate { get; set; }
		public int? IssueNumber { get; set; }
		public string VerificationCode { get; set; }
		public string PostCode { get; set; }
		public string TwoLetterISORegionName { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConsumingThirdPartyAPI.Models.DTO
{
    public class ApiCredentials
    {
        public int ApiCredentialsId { get; set; }
        [Required] 
        public string APIUrl { get; set; }
        [Required]  
        public string ConsumerKey { get; set; }
        [Required] 
        public string SecretKey { get; set; }
        [Required]
        public string APIkey { get; set; }
        [Required]
        public string TokenURl { get; set; }

        [Required]
        public bool IsValid { get; set; } = true;

        [Required]
        public bool IsDefault { get; set; } = false;
    }

    public class GetApiCredentials
    {
        public int SearchId { get; set; }
        [Required]
        public int Code { get; set; } = 0;
       
    }

    public class AccountBalance
    {
        [Required]
        public string MessageReference { get; set; } = "75e2bee7-0627-4d15-bb76-390dadf86b2a";
        [Required]
        public string AccountNumber { get; set; } = "36001873000";

    }
}
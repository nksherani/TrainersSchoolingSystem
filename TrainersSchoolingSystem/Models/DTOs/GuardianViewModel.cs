using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class GuardianViewModel
    {
        [Key]
        public int ParentId { get; set; }
        public string Name { get; set; }
        [RegularExpression("^[0-9]{5}-[0-9]{7}-[0-9]{1}$", ErrorMessage = "Enter CNIC in proper format like 12345-1234567-1")]
        public string CNIC { get; set; }
        public string Profession { get; set; }
        [Display(Name = "Organization Type")]
        public string OrganizationType { get; set; }
        public string Education { get; set; }
        [Display(Name = "Monthly Income")]
        public string MonthlyIncome { get; set; }
        [RegularExpression("^\\+[0-9]{12}$", ErrorMessage = "Enter Mobile No. in proper format like +923331234567")]
        [MaxLength(13, ErrorMessage = "Maximum characters limit exceeded")]
        public string Mobile { get; set; }
        [RegularExpression("(^\\+[0-9]{11}$)|(^\\+[0-9]{12}$)", ErrorMessage = "Enter Phone No. in proper format like +92211234567 or +922112345678")]
        [MaxLength(13, ErrorMessage = "Maximum characters limit exceeded")]
        public string Landline { get; set; }
        public string Address { get; set; }
        [Display(Name = "Office Phone")]
        public string OfficePhone { get; set; }
        [Display(Name = "Office Address")]
        public string OfficeAddress { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Relation { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        static IMapper toEntityMapper;
        static IMapper toModelMapper;
        static GuardianViewModel()
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<GuardianViewModel, Parent>();

            });
            toEntityMapper = config.CreateMapper();

            config = new MapperConfiguration(cfg => {

                cfg.CreateMap<Parent, GuardianViewModel>();

            });
            toModelMapper = config.CreateMapper();
        }
        public static Parent ToEntity(GuardianViewModel parentViewModel)
        {
            return toEntityMapper.Map<Parent>(parentViewModel);
        }
        public static GuardianViewModel ToModel(Parent parent)
        {
            return toModelMapper.Map<GuardianViewModel>(parent);
        }

    }
}
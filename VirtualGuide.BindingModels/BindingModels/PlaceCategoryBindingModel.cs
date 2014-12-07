using System.ComponentModel.DataAnnotations;

namespace VirtualGuide.BindingModels
{
    public class PlaceCategoryBindingModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(100, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [RegularExpression("^[a-z]{2}_[A-Z]{2}$",
            ErrorMessageResourceName = "InvalidLanguageFormat",
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Language { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string IconName { get; set; }

    }
}

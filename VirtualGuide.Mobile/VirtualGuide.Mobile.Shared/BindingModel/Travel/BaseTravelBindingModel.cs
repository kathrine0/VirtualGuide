using VirtualGuide.Mobile.Model;

namespace VirtualGuide.Mobile.BindingModel
{
    abstract public class BaseTravelBindingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public BaseTravelBindingModel(Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name;
        }

        public BaseTravelBindingModel()
        {

        }
    }
}

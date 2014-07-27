using Caliburn.Micro;
using CoreTweet;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public class UserEntititesModel : PropertyChangedBase, IUserEntities
    {
        public UserEntititesModel()
        { }

        public UserEntititesModel(UserEntities entities)
        {
            if (entities != null)
            {
                if (entities.Description != null)
                    Description = new EntitiesModel(entities.Description);
                if (entities.Url != null)
                    Url = new EntitiesModel(entities.Url);
            }
        }

        public IEntities Description { get; set; }
        public IEntities Url { get; set; }
    }
}

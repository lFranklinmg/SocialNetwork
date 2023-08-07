namespace Domain
{
    public class UserFollowing
    {
        //Followers
        public string ObserverID { get; set; }
        public AppUser Observer { get; set; }
        //Following
        public string TargetID{ get; set; }
        public AppUser Target { get; set; }

    }
}

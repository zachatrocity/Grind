using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Streams;
using System.Net.Http;
using Windows.UI.Xaml.Media.Imaging;
using Octokit;

namespace Grind
{
    class GitHubAPI
    {
        public string username = "";
        private GitHubClient github;
        private Octokit.User GithubUser;
        public GitHubAPI(string usr)
        {
            username = usr;
            github = new GitHubClient(new ProductHeaderValue("Grind"));
            
        }

        public async Task<int> getUsersPublicRepos()
        {
            var user = await github.User.Get(username);
            return user.PublicRepos;
        }

        public async Task<BitmapImage> getUserImage()
        {
            var user = await github.User.Get(username);

            var httpClient = new HttpClient();
            var contentBytes = await httpClient.GetByteArrayAsync(user.AvatarUrl);
            var ims = new InMemoryRandomAccessStream();
            var dataWriter = new DataWriter(ims);
            dataWriter.WriteBytes(contentBytes);
            await dataWriter.StoreAsync();
            ims.Seek(0);

            var bitmap = new BitmapImage();
            bitmap.SetSource(ims);
            
            return bitmap;
        }

        public async Task<string> getUserFollowing() {
            var user = await github.User.Get(username);
            return user.Following.ToString();
        }

        public async Task<string> getUserFollowers()
        {
            var user = await github.User.Get(username);
            return user.Followers.ToString();
        }

        public async Task<string> getUserRepoCount()
        {
            var user = await github.User.Get(username);
            return (user.PublicRepos + user.TotalPrivateRepos).ToString(); 
        }
    }
}

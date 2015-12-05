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
        private Octokit.User githubUser;
        public GitHubAPI(string usr)
        {
            username = usr;
            github = new GitHubClient(new ProductHeaderValue("Grind"));
            initGitHub();
        }

        private async void initGitHub()
        {
            try
            {
                githubUser = await github.User.Get(username);

            }
            catch
            {
            }
        }

        public async Task<int> getUsersPublicRepos()
        {
            if (githubUser != null)
            {
                return githubUser.PublicRepos;
            }
            else
            {
                githubUser = await github.User.Get(username);
                return githubUser.PublicRepos;
            }
            
        }

        public async Task<BitmapImage> getUserImage()
        {
            if (githubUser != null)
            {
                var httpClient = new HttpClient();
                var contentBytes = await httpClient.GetByteArrayAsync(githubUser.AvatarUrl);
                var ims = new InMemoryRandomAccessStream();
                var dataWriter = new DataWriter(ims);
                dataWriter.WriteBytes(contentBytes);
                await dataWriter.StoreAsync();
                ims.Seek(0);

                var bitmap = new BitmapImage();
                bitmap.SetSource(ims);
                return bitmap;
            }
            else
            {
                githubUser = await github.User.Get(username);
                var httpClient = new HttpClient();
                var contentBytes = await httpClient.GetByteArrayAsync(githubUser.AvatarUrl);
                var ims = new InMemoryRandomAccessStream();
                var dataWriter = new DataWriter(ims);
                dataWriter.WriteBytes(contentBytes);
                await dataWriter.StoreAsync();
                ims.Seek(0);

                var bitmap = new BitmapImage();
                bitmap.SetSource(ims);
                return bitmap;
            }
            
            
        }

        public async Task<string> getUserFollowing() {
            if (githubUser != null)
            {
                return githubUser.Following.ToString();
            }
            else
            {
                githubUser = await github.User.Get(username);
                return githubUser.Following.ToString();
            }
        }

        public async Task<string> getUserFollowers()
        {
            if (githubUser != null)
            {
                return githubUser.Followers.ToString();
            }
            else
            {
                githubUser = await github.User.Get(username);
                return githubUser.Followers.ToString();
            }
        }

        public async Task<string> getUserRepoCount()
        {
            if (githubUser != null)
            {
                return (githubUser.PublicRepos + githubUser.TotalPrivateRepos).ToString(); 
            }
            else
            {
                githubUser = await github.User.Get(username);
                return (githubUser.PublicRepos + githubUser.TotalPrivateRepos).ToString(); 
            }
            
        }
    }
}

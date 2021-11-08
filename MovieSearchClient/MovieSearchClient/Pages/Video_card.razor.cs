using System.Threading.Tasks;

using BLTools;

using Microsoft.AspNetCore.Components;

using MovieSearchClientServices;

using MovieSearchModels;

namespace MovieSearchClient.Pages {
  public partial class Video_card : ComponentBase {

    [Parameter]
    public IMovie Movie { get; set; }

    [Inject]
    public IMovieService MovieService { get; set; }
    
    public string HeaderText { get; set; }
    public string BodyTitle { get; set; }
    public string BodySubTitle { get; set; }
    public string BodyText { get; set; }
    public string FooterLink { get; set; }
    public string ImageSource { get; set; }
    public string ImageWidth { get; set; }


    protected override async Task OnParametersSetAsync() {
      HeaderText = Movie.LocalName;
      await GetImageSource();
      //if (Movie.LocalName.Contains("(")) {
      //  HeaderText = Movie.LocalName.BeforeLast('(').Trim();
      //} else {
      //  HeaderText = Movie.LocalName.BeforeLast('.').Trim();
      //}
    }

    private async Task GetImageSource() {
      IsImageLoaded = false;
      StateHasChanged();
      ImageSource = await MovieService.GetPicture64(Movie);
      IsImageLoaded = true;
      StateHasChanged();
    }

    private bool IsImageLoaded = false;

  }

}

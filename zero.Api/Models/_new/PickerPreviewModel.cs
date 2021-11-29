namespace zero.Api.Models;

public class PickerPreviewModel
{
  public string Id { get; set; }

  public string Icon { get; set; }

  public string Text { get; set; }

  public string Name { get; set; }

  //public static PickerPreviewModel NotFound(string id) => new()
  //{
  //  HasError = true,
  //  Icon = "fth-alert-circle color-red",
  //  Id = id,
  //  Name = "@errors.preview.notfound",
  //  Text = "@errors.preview.notfound_text"
  //};
}
namespace ProjectOne
{
  using System.ComponentModel.DataAnnotations;

  public class ClassOne
  {
    [Key]
    [MaxLength(450)]
    public string Id { get; set; }

    public string Value { get; set; }
  }
}
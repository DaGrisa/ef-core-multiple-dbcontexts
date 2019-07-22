namespace ProjectTwo
{
  using System.ComponentModel.DataAnnotations;

  public class ClassThree
  {
    [Key]
    [MaxLength(450)]
    public string Id { get; set; }

    public string Value { get; set; }
  }
}
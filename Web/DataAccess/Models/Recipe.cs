using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyRecipes.DataAccess.Models
{
    public partial class Recipe
    {

        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        
        public string PrepTime { get; set; }

        public string CookTime { get; set; }

        public int PrepTimeinMinutes { get; set; }

        public int CookTimeinMinutes { get; set; }

        [Required]
        public int Servings { get; set; }

        [Required]
        public DifficultyType Difficulty { get; set; }

        [Required]
        public CuisineType Cuisine { get; set; }

        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<Step> Steps { get; set; } = new List<Step>();

        public ApplicationUser User { get; set; }

        public string UserId { get; set; }

        public IEnumerable<FavoriteRecipe> FavoritedByUsers { get; set; }

        public Image Image { get; set; }
        public int? ImageId { get; set; }

        public Uri ImageUrl { get; set; }
    }
}
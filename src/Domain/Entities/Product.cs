using Domain.SeedWork;

namespace Domain.Entities;

public class Product : Aggregate<Guid>
{
    public string Name { get; set; } = default!;
    public string UrlSlug { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public Guid CategoryId { get; set; } = default!;
    public Category Category { get; set; } = default!;
    public List<ProductImage> Images { get; set; } = [];
    public List<ProductOption> Options { get; set; } = [];
    public List<Variant> Variants { get; set; } = [];

    public void AddOption(ProductOption productOption)
    {
        if (Options.Any(o => o.AllowImage) && productOption.AllowImage)
        {
            throw new InvalidOperationException("Only one product option can allow images per product.");
        }
        Options.Add(productOption);
    }

    public void RemoveOption(Guid optionId)
    {
        var option = Options.FirstOrDefault(o => o.Id == optionId);
        if (option == null)
        {
            throw new InvalidOperationException($"Option with ID {optionId} not found.");
        }
        Options.Remove(option);
    }

    public void AddOptionValue(Guid optionId, OptionValue optionValue, Image? image = null)
    {
        var option = Options.FirstOrDefault(o => o.Id == optionId);
        if (option == null)
        {
            throw new InvalidOperationException($"Option with ID {optionId} not found.");
        }

        if (!option.AllowImage && image != null)
        {
            throw new InvalidOperationException("Product option does not allow images.");
        }

        if (image != null)
        {
            optionValue.ImageId = image.Id;
            optionValue.Image = image;
        }

        option.AddValue(optionValue);
    }

    public void RemoveOptionValue(Guid optionId, Guid optionValueId)
    {
        var option = Options.FirstOrDefault(o => o.Id == optionId);
        if (option == null)
        {
            throw new InvalidOperationException($"Option with ID {optionId} not found.");
        }

        var value = option.Values.FirstOrDefault(v => v.Id == optionValueId);
        if (value == null)
        {
            throw new InvalidOperationException($"Option value with ID {optionValueId} not found.");
        }

        option.Values.Remove(value);
    }

    public void AddVariant(Variant variant)
    {
        Variants.Add(variant);
    }

    public void RemoveVariant(Guid variantId)
    {
        var variant = Variants.FirstOrDefault(v => v.Id == variantId);
        if (variant == null)
        {
            throw new InvalidOperationException($"Variant with ID {variantId} not found.");
        }
        Variants.Remove(variant);
    }

    public void UpdateVariant(Guid variantId, decimal? price = null, int? quantity = null, string? sku = null, bool? isActive = null)
    {
        var variant = Variants.FirstOrDefault(v => v.Id == variantId);
        if (variant == null)
        {
            throw new InvalidOperationException($"Variant with ID {variantId} not found.");
        }

        if (price.HasValue)
        {
            variant.Price = price.Value;
        }

        if (quantity.HasValue)
        {
            if (quantity.Value < 0)
            {
                throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
            }
            variant.Quantity = quantity.Value;
        }

        if (!string.IsNullOrWhiteSpace(sku))
        {
            variant.Sku = sku;
        }

        if (isActive.HasValue)
        {
            variant.IsActive = isActive.Value;
        }

        variant.UpdatedAt = DateTime.UtcNow;
    }

    public void BulkUpdateVariants(decimal? price = null, int? quantity = null, string? sku = null, bool? isActive = null)
    {
        foreach (var variant in Variants)
        {
            if (price.HasValue) variant.Price = price.Value;
            if (quantity.HasValue) variant.Quantity = quantity.Value;
            if (!string.IsNullOrWhiteSpace(sku)) variant.Sku = sku;
            if (isActive.HasValue) variant.IsActive = isActive.Value;
            variant.UpdatedAt = DateTime.UtcNow;
        }
    }

    public void AddImage(ProductImage productImage)
    {
        if (productImage.IsMain && Images.Any(i => i.IsMain))
        {
            throw new InvalidOperationException("Only one main image is allowed per product.");
        }
        Images.Add(productImage);
    }

    public void RemoveImage(Guid imageId)
    {
        var image = Images.FirstOrDefault(i => i.Id == imageId);
        if (image == null)
        {
            throw new InvalidOperationException($"Image with ID {imageId} not found.");
        }
        Images.Remove(image);
    }

    public void Update(string? name = null, string? description = null, Guid? categoryId = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            Name = name;
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            Description = description;
        }

        if (categoryId.HasValue)
        {
            CategoryId = categoryId.Value;
        }
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}
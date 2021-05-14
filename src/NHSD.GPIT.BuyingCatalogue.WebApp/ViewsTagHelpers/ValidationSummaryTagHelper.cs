﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.ViewsTagHelpers
{
    [HtmlTargetElement(TagHelperName)]
    public sealed class ValidationSummaryTagHelper : TagHelper
    {
        public const string TagHelperName = "nhs-validation-summary";
        public const string TitleName = "title";

        private const string DefaultTitle = "There is a problem";

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(TitleName)]
        public string Title { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output is null)            
                throw new ArgumentNullException(nameof(output));

            if (ViewContext.ViewData.ModelState.IsValid)
            {
                output.SuppressOutput();
                return;
            }

            var header = GetHeaderBuilder();
            var errorList = GetErrorListBuilder();

            BuildOutput(output);

            output.Content.AppendHtml(header);
            output.Content.AppendHtml(errorList);
        }

        private void BuildOutput(TagHelperOutput output)
        {
            output.TagName = TagHelperConstants.Div;
            output.TagMode = TagMode.StartTagAndEndTag;

            var attributes = new List<TagHelperAttribute>{
                new TagHelperAttribute(TagHelperConstants.Role, TagHelperConstants.RoleAlert),
                new TagHelperAttribute(TagHelperConstants.LabelledBy, TagHelperConstants.ErrorSummaryTitle),
                new TagHelperAttribute(TagHelperConstants.TabIndex, "-1"),
                new TagHelperAttribute(TagHelperConstants.Class, TagHelperConstants.NhsValidationSummary)
            };

            attributes.ForEach(a => output.Attributes.Add(a));
        }

        private static TagBuilder GetListItemBuilder(string linkElement, string errorMessage)
        {
            var listItemBuilder = new TagBuilder(TagHelperConstants.ListItem);
            var linkBuilder = new TagBuilder(TagHelperConstants.Anchor);
            linkBuilder.Attributes.Add(TagHelperConstants.Link, $"#{linkElement}");
            linkBuilder.InnerHtml.Append(errorMessage);
            listItemBuilder.InnerHtml.AppendHtml(linkBuilder);

            return listItemBuilder;
        }

        private TagBuilder GetHeaderBuilder()
        {
            var builder = new TagBuilder(TagHelperConstants.SubHeader);
            builder.AddCssClass(TagHelperConstants.NhsValidationSummaryTitle);
            builder.MergeAttribute(TagHelperConstants.Id, TagHelperConstants.ErrorSummaryTitle);
            builder.InnerHtml.Append(Title ?? DefaultTitle);

            return builder;
        }

        private TagBuilder GetErrorListBuilder()
        {
            var builder = new TagBuilder(TagHelperConstants.UnorderedList);
            builder.AddCssClass(TagHelperConstants.NhsList);
            builder.AddCssClass(TagHelperConstants.NhsValidationSummaryList);

            var viewType = ViewContext.ViewData.Model.GetType();

            if (viewType is null)            
                throw new InvalidOperationException();

            var propertyNames = viewType.GetProperties().Select(i => i.Name).ToList();
            var orderedStates = ViewContext.ViewData.ModelState
                .OrderBy(d => propertyNames.IndexOf(d.Key));

            foreach ((var key, ModelStateEntry modelStateEntry) in orderedStates)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    var listItem = GetListItemBuilder(key, error.ErrorMessage);
                    builder.InnerHtml.AppendHtml(listItem);
                }
            }

            return builder;
        }
    }
}

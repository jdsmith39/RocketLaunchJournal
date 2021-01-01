using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RocketLaunchJournal.Model.OwnedTypes
{
    [Owned]
    public class DocumentObj
    {
        [Column(nameof(DocumentFilename))]
        [Display(Name = "Filename On Disk")]
        [Required(ErrorMessage = Constants.ErrorMessages.RequiredField)]
        [StringLength(Constants.FieldSizes.DocumentFilenameLength, ErrorMessage = Constants.ErrorMessages.StringLengthMax)]
        public string? DocumentFilename { get; set; }

        [Column(nameof(DocumentDisplayName))]
        [Display(Name = "Filename")]
        [Required(ErrorMessage = Constants.ErrorMessages.RequiredField)]
        [StringLength(Constants.FieldSizes.DocumentDisplayNameLength, ErrorMessage = Constants.ErrorMessages.StringLengthMax)]
        public string? DocumentDisplayName { get; set; }

        [Column(nameof(MimeType))]
        [Display(Name = "Mime Type")]
        [Required(ErrorMessage = Constants.ErrorMessages.RequiredField)]
        [StringLength(Constants.FieldSizes.NameLength, ErrorMessage = Constants.ErrorMessages.StringLengthMax)]
        public string? MimeType { get; set; }
    }
}

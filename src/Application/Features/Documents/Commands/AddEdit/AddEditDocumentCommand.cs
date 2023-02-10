// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Documents.DTOs;
using CleanArchitecture.Blazor.Application.Features.Documents.Caching;
using CleanArchitecture.Core.Mappings;

namespace CleanArchitecture.Blazor.Application.Features.Documents.Commands.AddEdit;

public class AddEditDocumentCommand :IMapFrom<DocumentDto>, ICacheInvalidatorRequest<MethodResult<int>>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public string? URL { get; set; }
    public DocumentType DocumentType { get; set; } = DocumentType.Document;
    public string? TenantId { get; set; }
    public string? TenantName { get; set; }
    public CancellationTokenSource? SharedExpiryTokenSource => DocumentCacheKey.SharedExpiryTokenSource();
    public UploadRequest? UploadRequest { get; set; }
  
}

public class AddEditDocumentCommandHandler : IRequestHandler<AddEditDocumentCommand, MethodResult<int>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUploadService _uploadService;

    public AddEditDocumentCommandHandler(
        ApplicationDbContext context,
         IMapper mapper,
         IUploadService uploadService
        )
    {
        _context = context;
        _mapper = mapper;
        _uploadService = uploadService;
    }
    public async Task<MethodResult<int>> Handle(AddEditDocumentCommand request, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<DocumentDto>(request);
        if (request.Id > 0)
        {
            var document = await _context.Documents.FindAsync(new object[] { request.Id }, cancellationToken);
            _ = document ?? throw new NotFoundException($"Document {request.Id} Not Found.");
            document.AddDomainEvent(new UpdatedEvent<Document>(document));
            if (request.UploadRequest != null)
            {
                document.URL = await _uploadService.UploadAsync(request.UploadRequest);
            }
            document.Title = request.Title;
            document.Description = request.Description;
            document.IsPublic = request.IsPublic;
            document.DocumentType = request.DocumentType;
            await _context.SaveChangesAsync(cancellationToken);
            return await MethodResult<int>.SuccessAsync(document.Id);
        }
        else
        {
            var document = _mapper.Map<Document>(dto);
            if (request.UploadRequest != null)
            {
                document.URL = await _uploadService.UploadAsync(request.UploadRequest); ;
            }
             document.AddDomainEvent(new CreatedEvent<Document>(document));
            _context.Documents.Add(document);
            await _context.SaveChangesAsync(cancellationToken);
            return await MethodResult<int>.SuccessAsync(document.Id);
        }


    }
}

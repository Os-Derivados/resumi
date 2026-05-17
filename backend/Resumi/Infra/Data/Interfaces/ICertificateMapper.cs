using Resumi.Api.Data.Models;

namespace Resumi.Infra.Data.Interfaces;

public interface
    ICertificateMapper : IEntityMapper<Certificate, CertificateModel, AddCertificateModel, UpdateCertificateModel>;
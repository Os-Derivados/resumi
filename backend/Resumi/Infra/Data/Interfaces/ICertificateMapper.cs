using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;

namespace Resumi.Infra.Data.Interfaces;

public interface
    ICertificateMapper : IEntityMapper<Certificate, CertificateModel, AddCertificateModel, UpdateCertificateModel>;
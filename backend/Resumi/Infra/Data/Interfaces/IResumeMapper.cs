using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;

namespace Resumi.Infra.Data.Interfaces;

public interface IResumeMapper : IEntityMapper<Resume, ResumeModel, CreateResumeModel, UpdateResumeModel>;
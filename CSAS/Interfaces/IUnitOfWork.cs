using System;

namespace CSAS.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IStudentRepository Students { get; }
		ISettingsRepository Settings { get; }
		IAttachmentRepository Attachment { get; }
		IActivityRepository Activity { get; }
		ITaskTemplateRepository TasksTemplate { get; }
		ITaskRepository Task { get; }
		IActivityTemplateRepository ActivityTemplate { get; }
		IAttendanceRepository Attendance { get; }
		ISubGroupRepository SubGroup { get; }
		IMainGroupRepository MainGroup { get; }
		IFinalAssessmentRepository FinalAssessment { get; }
		IUserInfoRepository UserInfo { get; }

		void Complete();
	}
}

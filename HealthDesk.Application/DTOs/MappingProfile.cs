using AutoMapper;
using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.User.FullName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.User.FullName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.Clinic.Name))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(d => d.Specialty, o => o.MapFrom(s => s.Specialty));

            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email.ToString()));

            CreateMap<Clinic, ClinicDto>();
            CreateMap<Prescription, PrescriptionDto>()
                .ForMember(d => d.DoctorName,  o => o.MapFrom(s => s.Doctor.User.FullName))
                .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient.User.FullName));
        }
    }
}

using FluentValidation;
using AuthService.Application.Models.Requests.Auth;
using AuthService.Application.Repositories;

namespace AuthService.Application.Validators.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IUserRepository userRepository)
        {
            _ = RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ và tên là bắt buộc.");

            _ = RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Tên đăng nhập là bắt buộc.")
                .Length(3, 100).WithMessage("Tên đăng nhập phải có độ dài từ 3 đến 100 ký tự.")
                .Matches(@"^\S+$").WithMessage("Tên đăng nhập không được chứa khoảng trắng.")
                .MustAsync(async (userName, cancellation) => !await userRepository.ExistsByUserNameAsync(userName))
                .WithMessage("Tên đăng nhập đã được sử dụng.");

            _ = RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email là bắt buộc.")
                .EmailAddress().WithMessage("Định dạng email không hợp lệ.")
                .MaximumLength(255).WithMessage("Email phải nhỏ hơn 255 ký tự.")
                .Matches(@"^\S+$").WithMessage("Email không được chứa khoảng trắng.")
                .MustAsync(async (email, cancellation) => !await userRepository.ExistsByEmailAsync(email))
                .WithMessage("Email đã được sử dụng.");

            _ = RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu là bắt buộc.")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");

            _ = RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Xác nhận mật khẩu là bắt buộc.")
                .Equal(x => x.Password).WithMessage("Xác nhận mật khẩu phải khớp với mật khẩu.");

            _ = RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Địa chỉ là bắt buộc.")
                .MaximumLength(500).WithMessage("Địa chỉ phải nhỏ hơn 500 ký tự.");

            _ = RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Ngày sinh là bắt buộc.")
                .LessThan(DateTime.UtcNow).WithMessage("Ngày sinh phải là ngày trong quá khứ.");
        }
    }
}

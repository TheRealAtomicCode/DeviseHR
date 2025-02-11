import { useContext, useState } from 'react';
import FloatingLabelInput from '../components/Forms/FloatingInput';
import { useMutation } from '@tanstack/react-query';
import { TLoginData, login } from '../APIs/auth/Login';
import { TServiceResponse } from '../types/TServiceResponse';
import { useNavigate } from 'react-router-dom';
import { setCookie } from '../utils/cookies';
import { UserContext } from '../context/AppContext';

function LoginView() {
	const navigate = useNavigate();

	const context = useContext(UserContext);

	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');
	const [errorMessage, setErrorMessage] = useState('');

	const loginMutation = useMutation<
		TServiceResponse<TLoginData>,
		Error,
		{ email: string; password: string }
	>({
		mutationFn: ({ email, password }) => login({ email, password }),
		onSuccess: (res: TServiceResponse<TLoginData>) => {
			if (res.success) {
				setErrorMessage('');

				setCookie('jwt', res.data.jwt, 900);
				setCookie('refreshToken', res.data.refreshToken, 900);

				context.id = res.data.id;
				context.firstName = res.data.firstName;
				context.lastName = res.data.lastName;
				context.email = res.data.email;
				context.isTerminated = res.data.isTerminated;
				context.isVerified = res.data.isVerified;
				context.profilePicture = res.data.profilePicture;
				context.userRole = res.data.userRole;

				navigate('/');
			} else {
				setErrorMessage(res.message);
			}
		},
		onError: (error: Error) => {
			setErrorMessage(error.message);
		},
	});

	const handleSubmit = (e: React.FormEvent) => {
		e.preventDefault();

		loginMutation.mutate({ email, password });
	};

	return (
		<div className="flex min-h-screen">
			{/* Left side - Login form */}
			<div className="w-full md:w-1/2 flex items-center justify-center bg-white p-8">
				<div className="max-w-md w-full space-y-6">
					<h2 className="text-red-500">{errorMessage}</h2>
					<form className="space-y-4" onSubmit={handleSubmit}>
						<FloatingLabelInput
							label="Email"
							incorrectLabel="Not a valid email"
							type="email"
							regex={/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/i}
							value={email}
							onChange={(e) => setEmail(e.target.value)}
						/>
						<FloatingLabelInput
							label="Password"
							incorrectLabel="Please provide a strong password"
							type="password"
							regex={/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{10,}$/}
							value={password}
							onChange={(e) => setPassword(e.target.value)}
						/>
						<div>
							<button
								type="submit"
								className="w-full py-2 px-4 bg-blue-600 text-white font-semibold rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
							>
								{loginMutation.isPending ? 'Loading...' : 'Login'}
							</button>
						</div>
					</form>
				</div>
			</div>

			{/* Right side - Placeholder for your design */}
			<div className="hidden md:block w-1/2 bg-gray-200">
				{/* Put your design or image here */}
			</div>
		</div>
	);
}

export default LoginView;

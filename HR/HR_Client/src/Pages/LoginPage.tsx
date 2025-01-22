import { useState } from 'react';
import FloatingLabelInput from '../Components/Forms/FloatingInput';
import { useMutation } from '@tanstack/react-query';
import { ILoginData, ILoginRequestDto, login } from '../APIs/Login'
import { IServiceResponse } from '../Interfaces/IServiceResponse';
import { useNavigate } from 'react-router-dom';



function LoginView() {
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('')

  const loginMutation = useMutation<IServiceResponse<ILoginData>, Error, { email: string, password: string }>({
    mutationFn: ({ email, password }) => login({ email, password }),
    onSuccess: (data: IServiceResponse<ILoginData>) => {
      if (data.success) {
        setErrorMessage('');
        navigate('/');
      } else {
        setErrorMessage(data.message);
      }
    },
    onError: (error: Error) => {
      setErrorMessage(error.message);
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    loginMutation.mutate({ email, password})
  };

  return (
    <div className="flex min-h-screen">
      {/* Left side - Login form */}
      <div className="w-full md:w-1/2 flex items-center justify-center bg-white p-8">
        <div className="max-w-md w-full space-y-6">
          <h2 className='text-red-500'>{errorMessage}</h2>
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

import FloatingLabelInput from '../Components/Forms/FloatingInput'

function LoginView() {
  return (
    <div className="flex min-h-screen">
      {/* Left side - Login form */}
      <div className="w-full md:w-1/2 flex items-center justify-center bg-white p-8">
        <div className="max-w-md w-full space-y-6">
          <h1 className="text-4xl font-bold text-center text-blue-600">DeviseHR</h1>
          <form className="space-y-4">
  
            <FloatingLabelInput label='Email' type='email' incorrectLabel='Not a valid email' />
            <FloatingLabelInput label='Password' type='password' incorrectLabel='Please provide a strong password' />
            <div>
              <button
                type="submit"
                className="w-full py-2 px-4 bg-blue-600 text-white font-semibold rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
              >
                Login
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

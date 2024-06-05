import React from 'react';
import { toast } from 'react-toastify';

type ResetPasswordProps = {
    
};

const ResetPassword:React.FC<ResetPasswordProps> = () => {
    const [inputs, setInputs] = React.useState({email:""});

    const handleChangeInput = (e:React.ChangeEvent<HTMLInputElement>)=>{
        setInputs((prev)=>({...prev,[e.target.name]:e.target.value}));

    }

    const handleReset = async(e:React.FormEvent<HTMLFormElement>)=>{
        e.preventDefault();
        
        if(!inputs.email) return alert("Please fill all field");
        try {
            const API_URL = `http://localhost:5107/api/Accounts/ForgotPassword?email=${inputs.email}`
         
            const res = await fetch(API_URL, {
                method: "POST",
                mode: "cors",
                headers: {
                    'Accept': 'application/json, text/plain',
                    'Content-Type': 'application/json;charset=UTF-8'
                },
              });
            if(!res.ok) {
                toast.error(res.statusText, { position: "top-center", autoClose: 3000, theme: "dark" });
            }
            const result = await res.json();
 
            if(result.statusCode == 404) {
                toast.warn("Email does not exists",{ position: "top-center", autoClose: 3000, theme: "dark" })

            }else if(result.statusCode == 200){
                toast.success("Send request reset password successfully, please check your email",
                { position: "top-center", autoClose: 4000, theme: "dark" })
            }
        } catch (error:any) {
            toast.error(error.message,{ position: "top-center", autoClose: 3000, theme: "dark" })
        }
    }
    
    return <form className='space-y-6 px-6 lg:px-8 pb-4 sm:pb-6 xl:pb-8' onSubmit={handleReset}>
    <h3 className='text-xl font-medium  text-white'>Reset Password</h3>
    <p className='text-sm text-white '>
        Forgotten your password? Enter your e-mail address below, and we&apos;ll send you an e-mail allowing you
        to reset it.
    </p>
    <div>
        <label htmlFor='email' className='text-sm font-medium block mb-2 text-gray-300'>
            Your email
        </label>
        <input
            type='email'
            name='email'
            onChange={handleChangeInput}
            id='email'
            className='border-2 outline-none sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 bg-gray-600 border-gray-500 placeholder-gray-400 text-white'
            placeholder='name@company.com'
        />
    </div>

    <button
        type='submit'
        className={`w-full text-white  focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center 
        bg-brand-orange hover:bg-brand-orange-s `}
    >
        Reset Password
    </button>
</form>
}
export default ResetPassword;
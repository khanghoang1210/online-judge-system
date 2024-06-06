import "@/styles/globals.css";
import '@/styles/Submissions.module.css';
import type { AppProps } from "next/app";
import Head from "next/head";
import { useRouter } from "next/router";
import { useEffect } from "react";
import { RecoilRoot } from "recoil";
import { ToastContainer } from "react-toastify";

export default function App({ Component, pageProps }: AppProps) {
  const router = useRouter();

  useEffect(() => {
    const handleRouteChange = (url: any) => {
     
    };

    router.events.on("routeChangeComplete", handleRouteChange);

    return () => {
      router.events.off("routeChangeComplete", handleRouteChange);
    };
  }, [router.events]);

  return (
    <RecoilRoot>
			<Head>
				<title>CodeFast</title>
				<meta name='viewport' content='width=device-width, initial-scale=1' />
				<link rel='icon' href='/favicon.png'/>
				<meta
					name='description'
					content='Web application that contains leetcode problems and video solutions'
				/>
			</Head>
			<ToastContainer />
			<Component {...pageProps} />
		</RecoilRoot>
  );
}

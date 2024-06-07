import { vscodeDark } from "@uiw/codemirror-theme-vscode";
import { useState, useEffect } from "react";
import PreferenceNav from "./PreferenceNav/PreferenceNav";
import Split from "react-split";
import CodeMirror from "@uiw/react-codemirror";
import "react-toastify/dist/ReactToastify.css";
import { javascript } from "@codemirror/lang-javascript";
import { python } from "@codemirror/lang-python";
import EditorFooter from "./EditorFooter";
import { Problem } from "@/components/ProblemsTable/ProblemsTable";
import { toast } from "react-toastify";
import { useRouter } from "next/router";
import useLocalStorage from "@/hooks/useLocalStorage";
import { java } from "@codemirror/lang-java";
import { cpp } from "@codemirror/lang-cpp";
import { ProblemDetail } from "../ProblemDescription/ProblemDescription";
import { useCookies } from "react-cookie";
import { jwtDecode } from "jwt-decode";
import { Source_Code_Pro } from "next/font/google";

type PlaygroundProps = {
  problem: Problem;
  setSuccess: React.Dispatch<React.SetStateAction<boolean>>;
  setSolved: React.Dispatch<React.SetStateAction<boolean>>;
};

export interface ISettings {
  fontSize: string;
  settingsModalIsOpen: boolean;
  dropdownIsOpen: boolean;
}

const Playground: React.FC<PlaygroundProps> = ({
  problem,
  setSuccess,
  setSolved,
}) => {
  const [selectedLanguage, setSelectedLanguage] = useState("C++");
  const [activeTestCaseId, setActiveTestCaseId] = useState<number>(0);
  const [problemDetail, setProblem] = useState<ProblemDetail | null>(null);

  const [user, setUser] = useState<any>();
  const router = useRouter();
  const [cookie] = useCookies(["token"]);
  useEffect(() => {
    if (cookie.token) {
      setUser(jwtDecode(cookie.token));
    }
  }, [cookie.token]);
  const [fontSize, setFontSize] = useLocalStorage("lcc-fontSize", "16px");

  const [settings, setSettings] = useState<ISettings>({
    fontSize: fontSize,
    settingsModalIsOpen: false,
    dropdownIsOpen: false,
  });

  const [userCode, setUserCode] = useState<string>("");

  const {
    query: { pid },
  } = useRouter();

  useEffect(() => {
    const fetchProblemDetails = async () => {
      try {
        const response = await fetch(
          `http://localhost:5107/api/Problems/GetProblemDetail?problemId=${problem.problemId}`,
          {
            method: "GET",
            mode: "cors",
            headers: {
              Accept: "application/json, text/plain",
              "Content-Type": "application/json;charset=UTF-8",
            },
          }
        );
        const data = await response.json();
        setProblem(data.data);
      } catch (error) {
        toast.error("Failed to fetch problem details");
      }
    };

    fetchProblemDetails();
  }, [problem.problemId]);
  let initialCode;

  useEffect(() => {
    if (problemDetail) {
      if(problemDetail.returnType == 'int'){
        switch (selectedLanguage) {
          case "Python":
            initialCode = `def ${problemDetail.functionName}():`;
            break;
          case "Java":
            initialCode = `public static int ${problemDetail.functionName}() {}`;
            break;
          case "C++":
          default:
            initialCode = `int ${problemDetail.functionName}() {}`;
        }
        setUserCode(initialCode);
      }
      else if(problemDetail.returnType == 'string'){
        switch (selectedLanguage) {
          case "Python":
            initialCode = `def ${problemDetail.functionName}():`;
            break;
          case "Java":
            initialCode = `public static String ${problemDetail.functionName}() {}`;
            break;
          case "C++":
          default:
            initialCode = `string ${problemDetail.functionName}() {}`;
        }
        setUserCode(initialCode);
      }
      
      
    }
  }, [selectedLanguage, problemDetail]);
  const API_URL = "http://localhost:5107/api/Judge/Submit";
  const CREATE_API_URL = "http://localhost:5107/api/Submissions";

  const onChange = (value: string) => {
    setUserCode(value);
    localStorage.setItem(`code-${pid}`, JSON.stringify(value));
  };

  const handleLanguageChange = (language: string) => {
    setSelectedLanguage(language);
  };

  let languageMode;
  switch (selectedLanguage) {
    case "Python":
      languageMode = python();
      break;
    case "Java":
      languageMode = java();
      break;
    case "C++":
    default:
      languageMode = cpp();
  }
  const [res, setRes] = useState();
  const handleSubmit = async () => {
    if (!user) {
      console.log("errr");
      toast.error("Please login to submit", {
        position: "top-center",
        autoClose: 3000,
        theme: "dark",
      });
    }
    try{
      const data = {problemId: problem.problemId, sourceCode: userCode, language: selectedLanguage}
      let isPass = false;
      var  casesPass = 1;
      const res = await fetch(API_URL, {
          method: "POST",
          body: JSON.stringify(data),
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
 
      if(result.statusCode == 200) {
          console.log(cookie.token);
          setRes(result.data.output)
          toast.success("All test cases passed",{ position: "top-center", autoClose: 3000, theme: "dark" })
          const createData = {problemId:problem.problemId,language: selectedLanguage, isAccepted: true, numCasesPassed: problemDetail?.testCases.length }
          await fetch(CREATE_API_URL,{
            method: "POST",
            body: JSON.stringify(createData),
            mode: "cors",
            headers: {
                'Authorization': `Bearer ${cookie.token}`,
                'Content-Type': 'application/json;charset=UTF-8',
                'Accept': 'application/json, text/plain',
            },
          })
      
      }else if(result.statusCode == 204){
          setRes(result.data.output)
          toast.warn("One ore more test cases failed",{ position: "top-center", autoClose: 3000, theme: "dark" })
          const createData = {
            problemId:problem.problemId,
            language: selectedLanguage, 
            isAccepted: false, 
            numCasesPassed: (Number(problemDetail?.testCases.length))-1 
          }
          await fetch(CREATE_API_URL,{
            method: "POST",
            body: JSON.stringify(createData),
            mode: "cors",
            headers: {
                'Authorization': `Bearer ${cookie.token}`,
                'Accept': 'application/json, text/plain',
                'Content-Type': 'application/json;charset=UTF-8',
            },
          })
      }
      else if(result.statusCode == 400){
        setRes(result.data.output)
          toast.error("Compile Error",{ position: "top-center", autoClose: 3000, theme: "dark" })
      }
      else if(result.statusCode == 500){
        toast.error("Internal Error",{ position: "top-center", autoClose: 3000, theme: "dark" })
      }
      
      
     

    }
    catch(error:any){
      toast.error(error.message, {
        position: "top-center",
        autoClose: 3000,
        theme: "dark",
      })
    }
  };
  return (
    <div className="flex flex-col bg-dark-layer-1 relative overflow-x-hidden">
      <PreferenceNav
        settings={settings}
        setSettings={setSettings}
        onLanguageChange={handleLanguageChange}
      />

      <Split
        className="h-[calc(100vh-94px)]"
        direction="vertical"
        sizes={[60, 40]}
        minSize={60}
      >
        <div className="w-full overflow-auto">
          <CodeMirror
            value={userCode}
            theme={vscodeDark}
            onChange={onChange}
            extensions={[languageMode]}
            style={{ fontSize: settings.fontSize }}
          />
        </div>
        <div className="w-full px-5 overflow-auto">
          <div className="flex h-10 items-center space-x-6">
            <div className="relative flex h-full flex-col justify-center cursor-pointer">
              <div className="text-sm font-medium leading-5 text-white">
                Testcases
              </div>
              <hr className="absolute bottom-0 h-0.5 w-full rounded-full border-none bg-white" />
            </div>
          </div>

          <div className="flex">
            {problemDetail?.testCases.map((example, index) => (
              <div
                className={"mr-2 items-start mt-2 "}
                key={String(example.item1)}
                onClick={() => {if(index==0) setActiveTestCaseId(index)}}
              >
                <div className="flex flex-wrap items-center gap-y-4">
                  <div
                    className={`font-medium items-center transition-all focus:outline-none inline-flex bg-dark-fill-3 hover:bg-dark-fill-2 relative rounded-lg px-4 py-1 cursor-pointer whitespace-nowrap
                      ${activeTestCaseId === index ? "text-white" : "text-gray-500"}
                    `}
                  >
                    Case {index + 1}
                  </div>
                </div>
              </div>
            ))}
          </div>

          <div className="font-semibold my-4">
            <p className="text-sm font-medium mt-4 text-white">Expected Output: </p>
            <div className="w-full cursor-text rounded-lg border px-3 py-[10px] bg-dark-fill-3 border-transparent text-white mt-2">
              {problemDetail?.testCases[0].item2}
            </div>
            <p className="text-sm font-medium mt-4 text-white">Actual Output:</p>
            <div className="w-full cursor-text rounded-lg border px-3 py-[10px] bg-dark-fill-3 border-transparent text-white mt-2">
             {res}
            </div>
          </div>
        </div>
      </Split>
      <EditorFooter handleSubmit={handleSubmit} />
    </div>
  );
};
export default Playground;

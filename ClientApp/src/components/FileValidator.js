import {useState} from 'react';
import axios from 'axios';
import { toast} from 'react-toastify';

import './style.css';

  export const FileValidator = ({onSuccess}) => {
    const [files, setFiles] = useState([]);
    
    const [dll, setDll] = useState([]);
    const [images, setImg] = useState([]);
    const [lang, setLang] = useState([]);

    const [dllErrors, setDllErrors] = useState([]);
    const [imagesErrors, setImagesErrors] = useState([]);
    const [langErros, setLangErrors] = useState([]);

    const [isValidationFailed, setIsValidationFailed] = useState([]);
    const [isShowStructure, setIsShowStructure] = useState([]);

    const onInputChange = (e) => {
        setIsValidationFailed(true)
        setFiles(e.target.files)
        setIsShowStructure(true)
    };

    const onClickSave = (e) => {
        e.preventDefault();

        const data = new FormData();

        for(let i = 0; i < files.length; i++) {
            data.append('file', files[i]);
        }
       
        axios.post('//localhost:44466/file/save', data)
            .then((response) => {

                toast.success('Save Success');
                onSuccess(response.data)
            })
            .catch((e) => {
                toast.error('Save Error')
            })
    };

    const onSubmit = (e) => {
        e.preventDefault();
        setIsShowStructure(false)
        const data = new FormData();

        for(let i = 0; i < files.length; i++) {
            data.append('file', files[i]);
        }
       
        axios.post('//localhost:44466/file/', data)
            .then((response) => {
                
                setDll(response.data.dllsContent)
                setImg(response.data.imagesContent)
                setLang(response.data.languagesContent)
                
                setDllErrors(response.data.dllsErrors)
                setImagesErrors(response.data.imagesErrors)
                setLangErrors(response.data.languagesErrors)

                setIsValidationFailed(response.data.isValidationFailed)

                toast.success('Upload Success');
                onSuccess(response.data)
            })
            .catch((e) => {
                toast.error('Upload Error')
            })
    };

    return (
        <div>
        <h1>File Validator</h1>
        <p>Welcome to your smart file validator</p>
        <p>To get started, please follow the instruction given below,</p>
        <ul>
          <li><strong>Upload</strong>. Upload your zip file to the app</li>
          <li><strong>Validate</strong>. Validate the file</li>
          <li><strong>Save</strong>. Save the file</li>
        </ul>
        <hr/>
        <div class="formdiv">
        <form className="form" method="post" action="#" id="#" onSubmit={onSubmit}>
            <div className="form-group files">
                <label>Upload Your File </label>
                <input type="file"
                       onChange={onInputChange}
                       className="form-control"
                       multiple/>
            </div>
            {isValidationFailed && <button type="submit">Validate</button>}
            {!isValidationFailed && <button onClick={onClickSave}>Save</button>}
        </form>

        {!isValidationFailed && <label class="validatedLabel"> &nbsp;Successfully Validated&nbsp;</label>}
        </div>
        <div class="vl"></div>
        {!isShowStructure && <div className="fileStr">
        <br/>
            <div>
                <p><strong>Folder Strucuture</strong></p>
              
                    <p>Dlls</p>
                    <ul>
                        {dll.map(item => {
                            return <li>{item}</li>;
                        })}
                    </ul>
                    
                    {dllErrors.map(item => {
                            return <p style={{color:"red"}}><small>{item}</small></p>;
                        })}
                    <p>Images</p>
                    <ul>
                        {images.map(item => {
                            return <li>{item}</li>;
                        })}
                    </ul>
                    {imagesErrors.map(item => {
                            return <p style={{color:"red"}}><small>{item}</small></p>;
                        })}
                    <p>Languages</p>
                    <ul>
                        {lang.map(item => {
                            return <li>{item}</li>;
                        })}
                    </ul>
                    {langErros.map(item => {
                            return <p style={{color:"red"}}><small>{item}</small></p>;
                        })}
                </div>

        </div>}
        </div>
    )
};
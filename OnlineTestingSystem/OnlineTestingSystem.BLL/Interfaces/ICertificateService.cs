﻿using OnlineTestingSystem.BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTestingSystem.BLL.Interfaces
{
    public interface ICertificateService
    {
        IEnumerable<CertificateDTO> GetAllCertificates();

        IEnumerable<CertificateDTO> GetCertificatesByUserId(int userId);

        CertificateDTO GetCertificateById(int id);

        CertificateDTO GetCertificateByNumber(string number);

        void CreateCertificate(CertificateDTO certificate);

        void DeleteCertificate(int id);

    }
}
